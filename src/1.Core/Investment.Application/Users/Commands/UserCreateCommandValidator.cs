using FluentValidation;
using Investment.Application.Utilities.FluentValidations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Users.Commands;

public class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
{
    public UserCreateCommandValidator()
    {
        RuleFor(x => x.EmailAddress)
            .NotEmpty()
             .Must(NotContainSpaces).WithMessage("Email Address must not contain spaces.")
            .EmailAddress();

        RuleFor(x => x.Password)
            .PasswordRules();
    }
    private bool NotContainSpaces(string input) =>
        !input.Contains(' ');
}
