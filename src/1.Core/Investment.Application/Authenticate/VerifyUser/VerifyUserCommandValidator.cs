using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Investment.Application.Utilities.FluentValidations;

namespace Investment.Application.Authenticate.VerifyUser;

public class VerifyUserCommandValidator :
    AbstractValidator<VerifyUserCommand>
{
    public VerifyUserCommandValidator()
    {
        RuleFor(x => x.EmailAddress)
            .EmailAddress();

        RuleFor(x=> x.Token).
           VerifyTokenRules();
    }
}
