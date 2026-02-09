using MediatR;
using Microsoft.AspNetCore.Server.HttpSys;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Investment.Application.CryptoPayment.Commands.DepositWebhook;

public class DepositWebhookCommand : IRequest
{
    [JsonPropertyName("payment_id")]
    public long PaymentId { get; set; }

    [JsonPropertyName("invoice_id")]
    public long? InvoiceId { get; set; }

    [JsonPropertyName("payment_status")]
    public required string PaymentStatus { get; set; }

    [JsonPropertyName("pay_address")]
    public required string PayAddress { get; set; }

    [JsonPropertyName("payin_extra_id")]
    public string? PayinExtraId { get; set; }

    [JsonPropertyName("price_amount")]
    public decimal PriceAmount { get; set; }

    [JsonPropertyName("price_currency")]
    public required string PriceCurrency { get; set; }

    [JsonPropertyName("pay_amount")]
    public decimal PayAmount { get; set; }

    [JsonPropertyName("actually_paid")]
    public decimal ActuallyPaid { get; set; }

    [JsonPropertyName("pay_currency")]
    public required string PayCurrency { get; set; }

    [JsonPropertyName("order_id")]
    public string? OrderId { get; set; }

    [JsonPropertyName("order_description")]
    public string? OrderDescription { get; set; }

    [JsonPropertyName("purchase_id")]
    public string? PurchaseId { get; set; }

    [JsonPropertyName("outcome_amount")]
    public decimal OutcomeAmount { get; set; }

    [JsonPropertyName("outcome_currency")]
    public string? OutcomeCurrency { get; set; }

    [JsonPropertyName("payout_hash")]
    public string? PayoutHash { get; set; }

    [JsonPropertyName("payin_hash")]
    public string? PayinHash { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    // API returns the literal string "null", so keep it as string
    [JsonPropertyName("burning_percent")]
    public string? BurningPercent { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonIgnore]
    public int UserId { get; set; }
}

