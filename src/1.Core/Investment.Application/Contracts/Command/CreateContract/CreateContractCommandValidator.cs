using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Investment.Application.Utilities.Constants;

namespace Investment.Application.Contracts.Command.CreateContract;

public class CreateContractCommandValidator : AbstractValidator<CreateContractCommand>
{

    public CreateContractCommandValidator()
    {
        RuleFor(command => command.ImageFile)
            .NotEmpty()
            .Must(command => command.Length <= ContractConstants.MaxFileSizeImage && command.FileName.Length <= ContractConstants.MaxSizeImageName)
            .WithMessage($"Max File Size Image :{ContractConstants.MaxFileSizeImage} ,Max Size Image Name :{ContractConstants.MaxSizeImageName}  ");

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

        When(command => command.Metadate is not null, () =>
        {
            RuleFor(command => command.Metadate!.Label)
                .MaximumLength(10);
        });
    }
}
