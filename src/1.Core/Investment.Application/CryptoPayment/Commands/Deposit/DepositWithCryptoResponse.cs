using System.Text.Json.Serialization;

namespace Investment.Application.CryptoPayment.Commands.Deposit;

public class DepositWithCryptoResponse
{
    public required string PayAddress { get; set; }
    public required string PaymentId { get; set; }
    public required int transactionId { get;  set; }
    public decimal PayAmount { get; internal set; }
}

