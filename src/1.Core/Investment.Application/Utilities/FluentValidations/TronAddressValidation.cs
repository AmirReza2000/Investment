using System;
using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using NBitcoin.DataEncoders;

namespace Investment.Application.Utilities.FluentValidations;

public static class TronAddressValidation
{
    public static IRuleBuilderOptions<T, string> TronAddressRules<T>(
       this IRuleBuilder<T, string> rule)
    {
        return rule.Must(IsValidTronBase58);
     }
    public static bool IsValidTronBase58(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            return false;

        if (address.Length < 30 || address.Length > 40) return false; // typical range
        if (!address.StartsWith("T", StringComparison.Ordinal)) return false;

        try
        {
            byte[] payload = Encoders.Base58Check.DecodeData(address);

            return payload.Length == 21 && payload[0] == 0x41;
        }
        catch
        {
            return false;
        }
    }
   
}

