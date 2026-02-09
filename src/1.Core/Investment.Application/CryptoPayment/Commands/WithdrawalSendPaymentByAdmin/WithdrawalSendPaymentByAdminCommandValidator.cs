using FluentValidation;
using Investment.Application.CryptoPayment.Commands.WithdrawalSendPaymentByAdmin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.CryptoPayment.Commands.WithdrawalSendPaymentByAdmin;

public class WithdrawalSendPaymentByAdminCommandValidator : AbstractValidator<WithdrawalSendPaymentByAdminCommand>
{
    public WithdrawalSendPaymentByAdminCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty();

        RuleFor(command => command.TransactionId)
            .NotEmpty();
    }
}