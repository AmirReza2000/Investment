using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Investment.Api.Utilities;

[ApiController]
internal class ApiRouteAttribute : RouteAttribute
{
    public ApiRouteAttribute(
                  int version,
                  [StringSyntax("Route")] string route)
                   : base($"api/v{version}/{route}")
    {

    }
}
