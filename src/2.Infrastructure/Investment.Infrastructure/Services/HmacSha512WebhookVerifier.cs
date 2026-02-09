using Investment.Application.Utilities.Configs;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using System;
using System.Buffers.Text;
using System.IO;
using System.Reactive;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


namespace Investment.Infrastructure.Services;


public sealed class HmacSha512WebhookVerifier
{
    private readonly byte[] _secretKey;

    public HmacSha512WebhookVerifier(IOptions<CryptoPaymentConfig> options)
    {
        if (string.IsNullOrWhiteSpace(options.Value.IpnSecretKey))
            throw new ArgumentNullException(nameof(CryptoPaymentConfig.IpnSecretKey), "Secret must not be null/empty.");

        _secretKey = Encoding.UTF8.GetBytes(options.Value.IpnSecretKey);
    }

    //public HmacSha512WebhookVerifier(byte[] keyBytes)
    //{
    //    if (keyBytes is null) throw new ArgumentNullException(nameof(keyBytes));
    //    if (keyBytes.Length == 0) throw new ArgumentException("Key must not be empty.", nameof(keyBytes));
    //    _secretKey = (byte[])keyBytes.Clone();
    //}

    public bool Verify(string rawBody, string? providedSignature)
    {
        if (rawBody is null) throw new ArgumentNullException(nameof(rawBody));
        return Verify(Encoding.UTF8.GetBytes(rawBody), providedSignature);
    }

    public bool Verify(ReadOnlySpan<byte> rawBodyBytes, string? providedSignature)
    {
        if (string.IsNullOrWhiteSpace(providedSignature))
            return false;

        // Normalize common formats: "sha512=<sig>"
        var sig = providedSignature.Trim();
        const string prefix = "sha512=";
        if (sig.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            sig = sig.Substring(prefix.Length).Trim();

        // Decode signature: try HEX first, then Base64 as fallback
        if (!TryDecodeHex(sig, out var providedBytes))
        {
            if (!TryDecodeBase64(sig, out providedBytes))
                return false;
        }

        Span<byte> computed = stackalloc byte[64]; // SHA-512 output size

        using (var hmac = new HMACSHA512(_secretKey))
        {
            if (!hmac.TryComputeHash(rawBodyBytes, computed, out var written) || written != 64)
                return false;
        }

        // Constant-time compare on bytes
        return providedBytes.Length == 64 && CryptographicOperations.FixedTimeEquals(providedBytes, computed.ToArray());
    }

    private static bool TryDecodeHex(string hex, out byte[] bytes)
    {
        bytes = Array.Empty<byte>();

        // Remove spaces just in case
        hex = hex.Replace(" ", "", StringComparison.Ordinal);

        if (hex.Length != 128 || (hex.Length % 2) != 0) // 64 bytes => 128 hex chars
            return false;

        bytes = new byte[hex.Length / 2];
        return Convert.FromHexString(hex).AsSpan().TryCopyTo(bytes);
    }

    private static bool TryDecodeBase64(string s, out byte[] bytes)
    {
        bytes = Array.Empty<byte>();
        try
        {
            bytes = Convert.FromBase64String(s);
            return true;
        }
        catch
        {
            return false;
        }
    }
}



