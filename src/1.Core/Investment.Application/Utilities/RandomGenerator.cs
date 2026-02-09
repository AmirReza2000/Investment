using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Investment.Application.Utilities;

public static class RandomGenerator
{
    private const string Digits = "0123456789";
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private const string LowerAndDigits = "abcdefghijklmnopqrstuvwxyz0123456789";
    public static string GenerateString(int length = 20)
    {
        if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length));

        char[] result = new char[length];
        byte[] buffer = new byte[length];

        RandomNumberGenerator.Fill(buffer);

        for (int i = 0; i < length; i++)
        {
            result[i] = Chars[buffer[i] % Chars.Length];
        }

        return new string(result);
    }    
    public static string GenerateValidationToken(int length =20)
    {
        if (length < 1) throw new ArgumentOutOfRangeException(nameof(length));

        char[] result = new char[length];

        result[0] = Digits[RandomNumberGenerator.GetInt32(Digits.Length)];

        for (int i = 1; i < length; i++)
        {
            result[i] = LowerAndDigits[RandomNumberGenerator.GetInt32(LowerAndDigits.Length)];
        }

        return new string(result);
    }
}