using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Investment.Application.Utilities.Constants;

namespace Investment.Application.Utilities.FluentValidations;

public static class PasswordValidations
{
    public static IRuleBuilderOptions<T, string> PasswordRules<T>(
        this IRuleBuilder<T, string> rule)
    {
        return rule
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 12 characters.")
            .MaximumLength(128).WithMessage("Password must be at most 128 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain a lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain a digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain a special character.")
            .Must(NotContainSpaces).WithMessage("Password must not contain spaces.");
    }

    private static bool NotContainSpaces(string password)
        => !string.IsNullOrWhiteSpace(password) && !password.Contains(" ");

    public static IRuleBuilderOptions<T, string> VerifyTokenRules<T>(
            this IRuleBuilder<T, string> rule)
    {
       return rule.NotEmpty()
            .Length(AuthenticateConstants.EmailVerificationTokenLength)
            .Matches("[a-z]").WithMessage("token must contain a lowercase letter.")
            .Matches("[0-9]").WithMessage("token must contain a digit.");
    }
        }


