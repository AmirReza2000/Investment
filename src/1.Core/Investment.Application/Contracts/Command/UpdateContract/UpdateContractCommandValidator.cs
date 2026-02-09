using FluentValidation;
using Investment.Application.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Contracts.Command.UpdateContract;

public class UpdateContractCommandValidator : AbstractValidator<UpdateContractCommand>
{
    public UpdateContractCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();

        RuleFor(command => command.ImageName)
            .MaximumLength(ContractConstants.MaxSizeImageName);

        When(command => command.MaxAmount is not null, () =>
        {
            RuleFor(command => command.MinAmount)
            .LessThan(command => command.MaxAmount);

            RuleFor(command => command.MaxAmount)
            .GreaterThan(0);
        });

        RuleFor(command => command.MinAmount)
            .GreaterThan(0);

        RuleFor(command => command.MinDurationOfDay)
            .GreaterThanOrEqualTo((short)30);
        RuleFor(command => command.Title)
            .NotEmpty();

        RuleFor(command => command.Rate)
            .GreaterThan(0)
            .PrecisionScale(28, 2, false);

    }
}
