using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Investment.Application.Authenticate.ResendUserVerifyToken;

public class ResendUserVerifyTokenCommandValidator:AbstractValidator<ResendUserVerifyTokenCommand>
{
    public ResendUserVerifyTokenCommandValidator()
    {
        RuleFor(command => command.EmailAddress)
            .EmailAddress();
    }
}
