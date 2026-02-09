using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Investment.Application.Utilities.FluentValidations;

namespace Investment.Application.Authenticate.ChangePassword;

public class ChangePasswordCommandValidator:AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(command => command.CurrentPassword)
            .NotEmpty();

        RuleFor(command => command.NewPassword)
            .PasswordRules();

        RuleFor(command => command.RepeatNewPassword)
            .Equal(command => command.NewPassword)
            .WithMessage("Repeating the password is not the same as the password.");
    }
}
