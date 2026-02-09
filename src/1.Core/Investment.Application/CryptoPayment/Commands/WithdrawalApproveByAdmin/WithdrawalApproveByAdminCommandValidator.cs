using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.CryptoPayment.Commands.WithdrawalApproveByAdmin;

public class WithdrawalApproveByAdminCommandValidator : AbstractValidator<WithdrawalApproveByAdminCommand>
{
    public WithdrawalApproveByAdminCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty();

        RuleFor(command => command.TransactionId)
            .NotEmpty();
    }
}
