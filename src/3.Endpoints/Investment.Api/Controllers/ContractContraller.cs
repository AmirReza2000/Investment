using Investment.Api.Utilities;
using Investment.Application.Contracts.Command.ApproveUserContract;
using Investment.Application.Contracts.Command.CreateContract;
using Investment.Application.Contracts.Command.CreateContractMarketType;
using Investment.Application.Contracts.Command.CreateUserContract;
using Investment.Application.Contracts.Command.RejectUserContract;
using Investment.Application.Contracts.Command.UpdateContract;
using Investment.Application.Contracts.Command.UpdateMarketType;
using Investment.Application.Contracts.Queries.ContractMarketTypes;
using Investment.Application.Contracts.Queries.Contracts;
using Investment.Application.Contracts.Queries.ExpectedProfit;
using Investment.Application.Contracts.Queries.UserContractsForAdmin;
using Investment.Application.Contracts.Queries.UserContracts;
using Investment.Domain.Contracts.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Investment.Api.Controllers;

[ApiRoute(1, "Contracts")]
public class ContractController(IMediator mediator) : BaseController(mediator)
{
    #region commands

    [HttpPost("User")]
    [Authorize]
    //[Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
    public async Task<IActionResult> CreateUserContract([FromBody] CreateUserContractCommand command, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(command, cancellationToken));
    }

    #endregion

    #region queries

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetActiveContracts(ContractsQuery query, CancellationToken cancellationToken)
    {
        query.ActiveFilter = true;

        return Ok(await _mediator.Send(query, cancellationToken));
    }

    [Authorize]
    [HttpGet("{ContractId}")]
    public async Task<IActionResult> GetContract([FromRoute] ContractQuery query, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(query, cancellationToken));
    }

    [Authorize]
    [HttpGet("MarketTypes")]
    public async Task<IActionResult> GetActiveMarketTypes(CancellationToken cancellationToken)
    {
        var query = new ContractMarketTypesQuery(true);

        return Ok(await _mediator.Send(query, cancellationToken));
    }

    [Authorize]
    [HttpGet("User")]
    public async Task<IActionResult> GetUserContracts(UserContractsQuery query, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(query, cancellationToken));
    }

    [Authorize]
    [HttpGet("User/{UserContractId}")]
    public async Task<IActionResult> GetUserContract([FromRoute] UserContractQuery query, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(query, cancellationToken));
    }

    [Authorize]
    [HttpGet("ExpectedProfit")]
    public async Task<IActionResult> GetExpectedProfit(ExpectedProfitQuery query, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(query, cancellationToken));
    }


    #endregion

    #region For Admin

    [HttpPost("Admin")]
    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
    public async Task<IActionResult> CreateContract([FromForm] CreateContractCommand command, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(command, cancellationToken));
    }

    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
    [HttpPut("{ContractId:int}/Admin")]
    public async Task<IActionResult> UpdateContract(int ContractId, [FromForm] UpdateContractCommand command, CancellationToken cancellationToken)
    {
        command.Id = ContractId;

        await _mediator.Send(command, cancellationToken);

        return Ok();
    }

    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
    [HttpPost("MarketType/Admin")]
    public async Task<IActionResult> CreateContractMarketType([FromBody] CreateContractMarketTypeCommand command, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(command, cancellationToken));
    }

    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
    [HttpPut("MarketType/{MarketTypeId:int}/Admin")]
    public async Task<IActionResult> UpdateContractMarketType(int MarketTypeId, [FromBody] UpdateContractMarketTypeCommand command, CancellationToken cancellationToken)
    {
        command.Id = MarketTypeId;

        await _mediator.Send(command, cancellationToken);

        return Ok();
    }

    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
    [HttpPost("User/Approve/Admin")]
    public async Task<IActionResult> ApproveUserContract([FromBody] ApproveUserContractCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command);

        return Ok();
    }

    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
    [HttpPost("User/Reject/Admin")]
    public async Task<IActionResult> RejectUserContract([FromBody] RejectUserContractCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
    [HttpGet("Admin")]
    public async Task<IActionResult> GeContracts(ContractsQuery query, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(query, cancellationToken));
    }

    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
    [HttpGet("User/Admin")]
    public async Task<IActionResult> GeContractForAdmin([FromQuery] int? UserContractId, [FromQuery] int? UserId, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new UserContractsForAdminQuery(UserContractId, UserId), cancellationToken));
    }

    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
    [HttpGet("MarketTypes/Admin")]
    public async Task<IActionResult> GetMarketTypes(ContractMarketTypesQuery query, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(query, cancellationToken));
    }

    [Authorize]
    [HttpGet("{ContractId}/Admin")]
    public async Task<IActionResult> GetContractForAdmin([FromRoute] ContractQuery query, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(query, cancellationToken));
    }
    #endregion /ForAdmin
}
