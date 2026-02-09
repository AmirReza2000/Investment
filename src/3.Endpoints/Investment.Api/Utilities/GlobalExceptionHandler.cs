
namespace Investment.Api.Utilities;

using System;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Investment.Application.Utilities.Exceptions;
using Investment.Domain.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using static System.Runtime.InteropServices.JavaScript.JSType;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {

        ProblemDetails problem = exception switch
        {

            InvalidCommandException ex => CreateInvalidCommandProblemDetails(
                ex,
                httpContext),


            BusinessRuleValidationException ex => CreateBusinessRuleValidationProblemDetails(
                 ex,
                 httpContext),

            _ => CreateInternalServerErrorDetails(
                httpContext)
        };

        if (problem.Status == 500)
        {
            Log.Error(exception, "Unhandled exception caught by GlobalExceptionHandler");
        }

        httpContext.Response.StatusCode = problem.Status ?? 500;
        httpContext.Response.ContentType = "application/problem+json";
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }


    private BusinessRuleValidationExceptionProblemDetails CreateBusinessRuleValidationProblemDetails(
                BusinessRuleValidationException businessRuleValidationException,
                HttpContext httpContext)
    {
        var problemDetails =
                   new BusinessRuleValidationExceptionProblemDetails(businessRuleValidationException,
                    $"{httpContext.Request.Method} {httpContext.Request.Path}");


        return problemDetails;
    }

    private static InvalidCommandProblemDetails CreateInvalidCommandProblemDetails(
                        InvalidCommandException invalidCommandException,
                        HttpContext httpContext)
    {

        var problemDetails =
            new InvalidCommandProblemDetails(invalidCommandException,
              $"{httpContext.Request.Method} {httpContext.Request.Path}"
                );

        return problemDetails;
    }

    private static ProblemDetails CreateInternalServerErrorDetails(
        HttpContext ctx)
    {
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "server error",
            Detail = "An unexpected error occurred.",
            Type = "https://httpstatuses.com/500",
            Instance = $"{ctx.Request.Method} {ctx.Request.Path}"
        };

        problemDetails.Extensions["traceId"] = ctx.TraceIdentifier;

        return problemDetails;
    }


}


