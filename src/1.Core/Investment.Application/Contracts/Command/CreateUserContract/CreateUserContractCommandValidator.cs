using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Investment.Application.Contracts.Command.CreateUserContract;

public class CreateUserContractCommandValidator : AbstractValidator<CreateUserContractCommand>
{
    public CreateUserContractCommandValidator()
    {
        RuleFor(command => command.Amount)
                .GreaterThan(0)
                .PrecisionScale(28, 2, false);

        RuleFor(command => command.ContractDurationType)
                .NotEmpty()
                .IsInEnum();


        RuleFor(command => command.ContractId)
                .NotEmpty();


        RuleFor(command => command.ContractMarketTypeId)
                .NotEmpty();


        RuleFor(command => command.DurationOfDay)
                .NotEmpty()
                .GreaterThanOrEqualTo(30);
    }

}
