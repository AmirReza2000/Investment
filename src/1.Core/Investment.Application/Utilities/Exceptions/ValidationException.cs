using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.Resources;
using Investment.Domain.Common;

namespace Investment.Application.Utilities.Exceptions;

public class ValidationException : InvalidCommandException

{
    public new const string Title = "One or more validation errors occurred.";

    public const ErrorCodeEnum errorCode = ErrorCodeEnum.Validation;

    public ValidationException(Dictionary<string, string[]> errors)
        : base(
            title: Title,
            errors: errors,
            errorCode: errorCode
            )
    {
    }
}
