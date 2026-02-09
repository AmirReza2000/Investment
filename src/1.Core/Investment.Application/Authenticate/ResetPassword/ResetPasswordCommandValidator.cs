using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Investment.Application.Utilities.FluentValidations;

namespace Investment.Application.Authenticate.ResetPassword;

public class ResetPasswordCommandValidator:AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(command => command.NewPassword)
            .PasswordRules();

        RuleFor(command => command.EmailAddress)
            .EmailAddress();

        RuleFor(command => command.Token)
            .VerifyTokenRules();

    }
}
