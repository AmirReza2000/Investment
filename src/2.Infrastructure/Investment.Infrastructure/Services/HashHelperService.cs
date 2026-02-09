using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Investment.Domain.Users;
using Investment.Domain.Users.Entities;

namespace Investment.Infrastructure.Services;

public class HashHelperService : IHashHelperService
{
    public string HashPassword(string password)
    {
        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
        var saltPass = "EEkyZPH$I3AG$XCRPBFF7I7HeA2ZTWI!YSS^0&0";
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            Encoding.ASCII.GetBytes(saltPass),
            iterations,
            hashAlgorithm,
            keySize);
        return Convert.ToHexString(hash);
    }
    public string HashRefreshToken(string refreshToken)
    {
        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
        var saltPass = "EEkyZPH$I3AG$XCRPBFF7I7HeA2ZTWI!YSS^0&0";
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(refreshToken),
            Encoding.ASCII.GetBytes(saltPass),
            iterations,
            hashAlgorithm,
            keySize);
        return Convert.ToHexString(hash);
    }

    public string HashVerificationToken(string token)
    {
        const int keySize = 64;
        const int iterations = 100000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;
        var saltPass = "EEkyZPH$I3AG$XCRPBFF7I7HeA2ZTWI!YSS^0&0";
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(token),
            Encoding.ASCII.GetBytes(saltPass),
            iterations,
            hashAlgorithm,
            keySize);
        return Convert.ToHexString(hash);
    }
}
