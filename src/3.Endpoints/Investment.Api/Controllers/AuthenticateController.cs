using System.IdentityModel.Tokens.Jwt;
using Investment.Api.Utilities;
using Investment.Application.Authenticate.ChangePassword;
using Investment.Application.Authenticate.ChangePassword.Logout;
using Investment.Application.Authenticate.Login;
using Investment.Application.Authenticate.RefreshToken;
using Investment.Application.Authenticate.ResendUserVerifyToken;
using Investment.Application.Authenticate.ResetPassword;
using Investment.Application.Authenticate.VerifyUser;
using Investment.Application.Users.Commands;
using Investment.Domain.Users.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Investment.Api.Controllers;

[ApiRoute(1, "Authenticate")]
public class AuthenticateController(IMediator mediator) : Controller
{
    #region Commadns

    [HttpPost("ResendUserVerifyToken")]
    public async Task<IActionResult> ResendUserVerifyTokenAsync([FromBody] ResendUserVerifyTokenCommand command)
    {
        await mediator.Send(command);

        return Ok();
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginCommand command)
    {
        var loginResponse = await mediator.Send(command);

        return Ok(loginResponse);
    }

    [AllowAnonymous]
    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var response = await mediator.Send(command);

        if (response == null)
        {
            return Unauthorized();
        }
        return Ok(response);
    }

    [HttpPost("VerifyUser")]
    public async Task<IActionResult> VerifyUserAsync([FromBody] VerifyUserCommand command)
    {
        await mediator.Send(command);

        return Ok();
    }

    [HttpPost("RequestResetPassword")]
    public async Task<IActionResult> RequestResetPassword([FromBody] SendEmailResetPasswordCommand command)
    {
        await mediator.Send(command);

        return Ok();
    }

    [HttpPost("VerifyTokenResetPassword")]
    public async Task<IActionResult> VerifyTokenResetPassword([FromBody] VerifyTokenResetPasswordCommand command)
    {
        await mediator.Send(command);
        return Ok();
    }

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        await mediator.Send(command);

        return Ok();
    }

    [Authorize]
    [HttpPost("ChangePassword")]    
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        await mediator.Send(command);

        return Ok();
    }

    [Authorize]
    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        await mediator.Send(new LogoutCommand());

        return Ok();
    }
    #endregion /Commands
}
