using FluentValidation;
using Investment.Application.Utilities.FluentValidations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Authenticate.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.EmailAddress)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");

            RuleFor(x => x.Password)
                .PasswordRules();
        }
    }
}
