using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Investment.Application.Utilities.FluentValidations;

namespace Investment.Application.CryptoPayment.Commands.Withdrawal;

public class WithdrawalCommandValidator : AbstractValidator<WithdrawalCommand>
{
    public WithdrawalCommandValidator()
    {
        RuleFor(e => e.AddressWithdrawal)
            .TronAddressRules();

        RuleFor(e => e.Amount)
            .GreaterThan(10)
            .Must(amount => DecimalPlaces(amount) <= 6)
            .WithMessage("Amount must not exceed 6 decimal places.");
    }
    private static int DecimalPlaces(decimal value)
    {
        value = Math.Abs(value);
        return BitConverter.GetBytes(decimal.GetBits(value)[3])[2];
    }
}
