using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Investment.Application.Utilities.DTO.Nowpayment;

public class MinimumPaymentAmountResponse
{
    [JsonPropertyName("currency_from")]
    public required string CurrencyFrom { get; set; }

    [JsonPropertyName("currency_to")]
    public required string CurrencyTo { get; set; }

    [JsonPropertyName("min_amount")]
    public required decimal MinAmount { get; set; }

    [JsonPropertyName("fiat_equivalent")]
    public required decimal FiatEquivalent { get; set; }
}


