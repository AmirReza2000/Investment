using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Investment.Application.Utilities.DTO.Nowpayment;

public class CreateCryptoDepositRequest
{
    [JsonPropertyName("price_amount")]
    public required string PriceAmount { get; set; } 

    [JsonPropertyName("price_currency")]
    public required string PriceCurrency { get; set; } 

    [JsonPropertyName("pay_currency")]
    public required string PayCurrency { get; set; } 

    [JsonPropertyName("ipn_callback_url")]
    public required string IpnCallbackUrl { get; set; } 

    [JsonPropertyName("order_id")]
    public string? OrderId { get; set; } 

    [JsonPropertyName("order_description")]
    public string? OrderDescription { get; set; } 

    [JsonPropertyName("is_fixed_rate")]
    public required bool IsFixedRate { get; set; }

    [JsonPropertyName("is_fee_paid_by_user")]
    public required bool IsFeePaidByUser { get; set; }
}
