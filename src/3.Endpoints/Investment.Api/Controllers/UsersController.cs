using Investment.Application.Users.Commands;
using Investment.Api.Utilities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Investment.Application.Users.Queries.Dashboard;
using Microsoft.AspNetCore.Authorization;

namespace Investment.Api.Controllers;

[ApiRoute(1, "Users")]
public class UsersController(IMediator mediator) : Controller
{
    #region Queries

    [Authorize]
    [HttpGet("Dashboard")]
    public async Task<IActionResult> GetDashboardInfo()
    {
        return Ok(await mediator.Send(new DashboardQuery()));

    }


    #endregion /Queries

    #region Commands

    [HttpPost("Register")]
    public async Task<IActionResult> CreateUserAsync([FromBody] UserCreateCommand command)
    {
        return Ok(await mediator.Send(command));
    }

    #endregion Commands
}
