using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Investment.Application.Utilities.DTO.Nowpayment;

public class CreateCryptoDepositResponse
{

    [JsonPropertyName("payment_id")]
    public required string PaymentId { get; set; }

    [JsonPropertyName("payment_status")]
    public string? PaymentStatus { get; set; }

    [JsonPropertyName("pay_address")]
    public required string PayAddress { get; set; }

    [JsonPropertyName("price_amount")]
    public decimal PriceAmount { get; set; }

    [JsonPropertyName("price_currency")]
    public string? PriceCurrency { get; set; }

    [JsonPropertyName("pay_amount")]
    public decimal PayAmount { get; set; }

    [JsonPropertyName("amount_received")]
    public decimal AmountReceived { get; set; }

    [JsonPropertyName("pay_currency")]
    public string? PayCurrency { get; set; }

    [JsonPropertyName("order_id")]
    public string? OrderId { get; set; }

    [JsonPropertyName("order_description")]
    public string? OrderDescription { get; set; }

    [JsonPropertyName("ipn_callback_url")]
    public string? IpnCallbackUrl { get; set; }

    [JsonPropertyName("customer_email")]
    public string? CustomerEmail { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }

    [JsonPropertyName("purchase_id")]
    public string? PurchaseId { get; set; }

    [JsonPropertyName("smart_contract")]
    public string? SmartContract { get; set; }

    [JsonPropertyName("network")]
    public string? Network { get; set; }

    [JsonPropertyName("network_precision")]
    public int? NetworkPrecision { get; set; }

    [JsonPropertyName("time_limit")]
    public int? TimeLimit { get; set; }

    [JsonPropertyName("burning_percent")]
    public decimal? BurningPercent { get; set; }

    [JsonPropertyName("expiration_estimate_date")]
    public DateTimeOffset ExpirationEstimateDate { get; set; }

    [JsonPropertyName("is_fixed_rate")]
    public bool IsFixedRate { get; set; }

    [JsonPropertyName("is_fee_paid_by_user")]
    public bool IsFeePaidByUser { get; set; }

    [JsonPropertyName("valid_until")]
    public DateTimeOffset ValidUntil { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("product")]
    public string? Product { get; set; }

    [JsonPropertyName("origin_ip")]
    public string? OriginIp { get; set; }
}
