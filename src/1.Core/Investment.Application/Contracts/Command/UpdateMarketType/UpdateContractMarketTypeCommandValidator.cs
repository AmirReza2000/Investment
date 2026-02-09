using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Investment.Application.Contracts.Command.CreateContractMarketTye;

namespace Investment.Application.Contracts.Command.UpdateMarketType;

public class UpdateContractMarketTypeCommandValidator : AbstractValidator<UpdateContractMarketTypeCommand>
{
    public UpdateContractMarketTypeCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();

        RuleFor(command => command.Title)
            .NotEmpty();

        RuleFor(command => command.Rate)
            .PrecisionScale(28, 2, false)
            .GreaterThan(0)
;

        RuleFor(command => command.IsActive)
            .NotNull();
    }
}

