using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.CryptoPayment.Commands.WithdrawalVerifyByAdmin;

public class WithdrawalVerifyByAdminCommand : IRequest
{
    public required int TransactionId { get; set; }

    public required int UserId { get; set; }

    public required string VerificationCode { get; set; }
}
