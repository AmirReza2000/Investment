using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Investment.Application.Utilities.DTO.Nowpayment;

public class CreateBatchCryptoWithdrawalResponse
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("withdrawals")]
    public required List<WithdrawalResponse> Withdrawals { get; set; }
}

public class WithdrawalResponse
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("address")]
    public required string Address { get; set; }

    [JsonPropertyName("currency")]
    public required string Currency { get; set; }

    // API sends numeric values as strings
    [JsonPropertyName("amount")]
    public required string Amount { get; set; }

    [JsonPropertyName("fiat_amount")]
    public string? FiatAmount { get; set; }

    [JsonPropertyName("fiat_currency")]
    public string? FiatCurrency { get; set; }

    [JsonPropertyName("batch_withdrawal_id")]
    public required string BatchWithdrawalId { get; set; }

    [JsonPropertyName("ipn_callback_url")]
    public required string IpnCallbackUrl { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("extra_id")]
    public string? ExtraId { get; set; }

    [JsonPropertyName("hash")]
    public string? Hash { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("payout_description")]
    public string? PayoutDescription { get; set; }

    [JsonPropertyName("unique_external_id")]
    public string? UniqueExternalId { get; set; }

    // snake_case version
    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("requested_at")]
    public DateTime? RequestedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // camelCase version (some objects use this instead)
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAtCamel { get; set; }

    [JsonPropertyName("requestedAt")]
    public DateTime? RequestedAtCamel { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAtCamel { get; set; }
}