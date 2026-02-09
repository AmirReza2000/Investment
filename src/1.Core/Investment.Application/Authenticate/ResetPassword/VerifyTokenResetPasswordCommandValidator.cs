using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Investment.Application.Utilities.FluentValidations;

namespace Investment.Application.Authenticate.ResetPassword;

public class VerifyTokenResetPasswordCommandValidator:
    AbstractValidator<VerifyTokenResetPasswordCommand>
{
    public VerifyTokenResetPasswordCommandValidator()
    {
        RuleFor(command => command.EmailAddress).
            EmailAddress();


        RuleFor(x => x.Token).
           VerifyTokenRules();
    }
}
