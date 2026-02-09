using Investment.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.CryptoPayment.Commands.WithdrawalApproveByAdmin;

public class WithdrawalApproveByAdminCommand : IRequest
{
    public required int TransactionId { get; set; }

    public required int UserId { get; set; }
}
