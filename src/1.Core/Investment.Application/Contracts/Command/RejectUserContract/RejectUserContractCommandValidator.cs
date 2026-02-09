using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Investment.Application.Contracts.Command.RejectUserContract;

public class RejectUserContractCommandValidator : AbstractValidator<RejectUserContractCommand>
{
    public RejectUserContractCommandValidator()
    {
        RuleFor(command => command.ContractId)
            .NotEmpty();

        RuleFor(command => command.UserContractId)
            .NotEmpty();

        RuleFor(command => command.Description)
            .NotEmpty()
            .MaximumLength(100);
    }
}
