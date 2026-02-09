namespace Investment.Application.CryptoPayment.Commands.WithdrawalSendPaymentByAdmin;

public class WithdrawalSendPaymentByAdminResponse
{
    public required string PaymentId { get; set; }
    public required string AmountCurrency { get; set; }
}