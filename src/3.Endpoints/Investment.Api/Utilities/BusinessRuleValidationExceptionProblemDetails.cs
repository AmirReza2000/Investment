using System.Net.NetworkInformation;
using Investment.Domain.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Investment.Api.Utilities;

public class BusinessRuleValidationExceptionProblemDetails : ProblemDetails
{
    public BusinessRuleValidationExceptionProblemDetails(BusinessRuleValidationException exception, string instance)
    {
        Title = "Business rule broken";
        Status = StatusCodes.Status409Conflict;
        Detail = exception.Message;
        Type = "https://httpstatuses.com/409";
        Instance = instance;
        Extensions["BusinessRuleCode"] = exception.BrokenRule.BusinessRuleCode;

    }
}
