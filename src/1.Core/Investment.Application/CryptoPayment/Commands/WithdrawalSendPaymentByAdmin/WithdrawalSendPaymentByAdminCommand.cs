using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.CryptoPayment.Commands.WithdrawalSendPaymentByAdmin;

public class WithdrawalSendPaymentByAdminCommand : IRequest<WithdrawalSendPaymentByAdminResponse>
{
    public required int TransactionId { get; set; }

    public required int UserId { get; set; }
}
