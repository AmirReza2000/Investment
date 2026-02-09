using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Infrastructure.Services;

public class HangfireBasicAuthAuthorizationFilter : IDashboardAuthorizationFilter
{
    private readonly string _username;
    private readonly string _password;

    public HangfireBasicAuthAuthorizationFilter(string username, string password)
    {
        _username = username;
        _password = password;
    }

    public bool Authorize(DashboardContext context)
    {

        var httpContext = context.GetHttpContext();
        var header = httpContext.Request.Headers["Authorization"];

        if (string.IsNullOrWhiteSpace(header))
        {
            Challenge(httpContext);
            return false;
        }

        var authHeader = header.ToString();
        if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            Challenge(httpContext);
            return false;
        }

        var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
        var decodedBytes = Convert.FromBase64String(encodedCredentials);
        var decodedString = Encoding.UTF8.GetString(decodedBytes);
        var parts = decodedString.Split(':');

        if (parts.Length != 2)
        {
            Challenge(httpContext);
            return false;
        }

        var username = parts[0];
        var password = parts[1];

        if (username == _username && password == _password)
        {
            return true;
        }

        Challenge(httpContext);
        return false;
    }

    private void Challenge(HttpContext httpContext)
    {
        httpContext.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Hangfire Dashboard\"";
        httpContext.Response.StatusCode = 401;
    }
}
