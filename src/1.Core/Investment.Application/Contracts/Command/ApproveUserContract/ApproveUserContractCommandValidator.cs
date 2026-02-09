using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Investment.Application.Contracts.Command.ApproveUserContract;

public class ApproveUserContractCommandValidator:AbstractValidator<ApproveUserContractCommand>
{
    public ApproveUserContractCommandValidator()
    {
        RuleFor(command => command.UserContractId)
            .NotEmpty();
    }
}
