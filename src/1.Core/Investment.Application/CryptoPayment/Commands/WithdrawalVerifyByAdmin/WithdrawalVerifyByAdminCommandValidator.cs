using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.CryptoPayment.Commands.WithdrawalVerifyByAdmin;

public class WithdrawalVerifyByAdminCommandValidator : AbstractValidator<WithdrawalVerifyByAdminCommand>
{
    public WithdrawalVerifyByAdminCommandValidator()
    {
        RuleFor(entity => entity.TransactionId)
            .NotEmpty();

        RuleFor(entity => entity.UserId)
            .NotEmpty();

        RuleFor(entity=>entity.VerificationCode)
            .NotEmpty()
            .Matches("[0-9]");
    }
}
