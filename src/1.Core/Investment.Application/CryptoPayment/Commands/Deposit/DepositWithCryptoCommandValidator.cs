using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.CryptoPayment.Commands.Deposit;

public class DepositWithCryptoCommandValidator : AbstractValidator<DepositWithCryptoCommand>
{
    public DepositWithCryptoCommandValidator()
    {
        RuleFor(x => x.PriceAmount)
            .GreaterThan(10)
            .LessThan(843)
            .PrecisionScale(28,2,false);        
    }
}
