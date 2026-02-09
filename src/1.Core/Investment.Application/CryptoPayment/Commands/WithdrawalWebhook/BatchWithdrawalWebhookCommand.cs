using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using MediatR;

namespace Investment.Application.CryptoPayment.Commands.WithdrawalWebhook;

public class BatchWithdrawalWebhookCommand:IRequest
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("address")]
    public required string Address { get; set; }

    [JsonPropertyName("currency")]
    public required string Currency { get; set; }

    [JsonPropertyName("amount")]
    public required string Amount { get; set; }

    [JsonPropertyName("batch_withdrawal_id")]
    public required string BatchWithdrawalId { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("extra_id")]
    public string? ExtraId { get; set; }

    [JsonPropertyName("hash")]
    public string? Hash { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("requestedAt")]
    public DateTime? RequestedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public int UserId { get; set; }
}