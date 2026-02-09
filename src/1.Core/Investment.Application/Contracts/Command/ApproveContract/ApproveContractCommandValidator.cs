using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Investment.Application.Contracts.Command.ApproveContract;

public class ApproveContractCommandValidator:AbstractValidator<ApproveContractCommand>
{
    public ApproveContractCommandValidator()
    {
        RuleFor(command => command.UserContractId)
            .NotEmpty();
    }
}
