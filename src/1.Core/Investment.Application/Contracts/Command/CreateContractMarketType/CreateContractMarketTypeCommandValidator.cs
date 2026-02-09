using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Investment.Application.Contracts.Command.CreateContractMarketType;

namespace Investment.Application.Contracts.Command.CreateContractMarketTye;

public class CreateContractMarketTypeCommandValidator: AbstractValidator<CreateContractMarketTypeCommand>
{
    public CreateContractMarketTypeCommandValidator()
    {
        RuleFor(command=>command.Title)
            .NotEmpty();

        RuleFor(command => command.Rate)
            .GreaterThan(0)
            .PrecisionScale(28, 2, false);
    }
}
