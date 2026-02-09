using System.Net.NetworkInformation;
using FluentValidation;
using Investment.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Investment.Api.Utilities;

public class FluentValidationExceptionProblemDetails : ProblemDetails
{
    public FluentValidationExceptionProblemDetails(ValidationException exception)
    {
        Title = "Error validating the entered data ";
        Status = StatusCodes.Status400BadRequest;
        Detail = exception.Message;
        Type = "https://httpstatuses.com/400";
    }
}