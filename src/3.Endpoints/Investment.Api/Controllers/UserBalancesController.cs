using Investment.Api.Utilities;
using Investment.Application.UserBalances.Commands.CreateUserBalance;
using Investment.Application.UserBalances.Queries.GetTransactions;
using Investment.Application.UserBalances.Queries.GetUserBalance;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Investment.Api.Controllers;

[ApiRoute(1, "UserBalances")]
[Authorize]
public class UserBalancesController : BaseController
{
    public UserBalancesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserBalance(CancellationToken cancellationToken)
    {
        await _mediator.Send(request: new CreateUserBalanceCommand(), cancellationToken);
        return Ok();
    }


    [HttpGet]
    public async Task<IActionResult> GetUserBalance(CancellationToken cancellationToken)
    {
       var response =await _mediator.Send(new GetUserBalanceQuery(),cancellationToken);

        return Ok(response);
    }

    [HttpGet("transactions")]
    public async Task<IActionResult> GetUserBalanceWithTransactionsAsync(
        GeUserBalanceTransactionsQuery getTransactionsQuery ,CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(getTransactionsQuery, cancellationToken);
        return Ok(response);
    }
}
