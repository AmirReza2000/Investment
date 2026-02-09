using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Investment.Application.Utilities.DTO.Nowpayment;

public class CreateBatchCryptoWithdrawalRequest
{
    [JsonPropertyName("withdrawals")]
    public required List<WithdrawalRequest> Withdrawals { get; set; }


}

public sealed class WithdrawalRequest
{
    [JsonPropertyName("address")]
    public required string Address { get; set; }

    [JsonPropertyName("currency")]
    public required string Currency { get; set; }

    [JsonPropertyName("amount")]
    public required string Amount { get; set; }

    [JsonPropertyName("fiat_amount")]
    public required string FiatAmount { get; set; }

    [JsonPropertyName("fiat_currency")]
    public required string FiatCurrency { get; set; }

    [JsonPropertyName("ipn_callback_url")]
    public required string IpnCallbackUrl { get; set; }

    [JsonPropertyName("payout_description")]
    public required string Description { get; set; }
}
