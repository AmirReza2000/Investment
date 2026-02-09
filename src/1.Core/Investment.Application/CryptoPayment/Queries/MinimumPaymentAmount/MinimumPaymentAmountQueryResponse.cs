using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.CryptoPayment.Queries.MinimumPaymentAmount;

public class MinimumPaymentAmountQueryResponse
{
    public string? CurrencyTo { get; internal set; }
    public string? CurrencyFrom { get; internal set; }
    public decimal MinAmount { get; internal set; }
    public decimal FiatEquivalent { get; internal set; }
}
