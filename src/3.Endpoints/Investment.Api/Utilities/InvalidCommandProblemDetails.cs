using System.Net.NetworkInformation;
using Investment.Application.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Investment.Api.Utilities;

public class InvalidCommandProblemDetails : ProblemDetails
{
    public InvalidCommandProblemDetails(InvalidCommandException exception, string instance)
    {
        Title = exception.Title;
        Status = StatusCodes.Status400BadRequest;
        Instance=instance;
        Detail = $"A {exception.ErrorCode} error was thrown";
        Extensions["errorCode"]= exception.ErrorCode;
        Extensions["errors"]= exception.Errors;
    }
}