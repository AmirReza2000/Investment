using System.Text;
using System.Text.Json;
using Investment.Api.Utilities;
using Investment.Application.CryptoPayment.Commands.Deposit;
using Investment.Application.CryptoPayment.Commands.DepositWebhook;
using Investment.Application.CryptoPayment.Commands.Withdrawal;
using Investment.Application.CryptoPayment.Commands.WithdrawalApproveByAdmin;
using Investment.Application.CryptoPayment.Commands.WithdrawalSendPaymentByAdmin;
using Investment.Application.CryptoPayment.Commands.WithdrawalVerifyByAdmin;
using Investment.Application.CryptoPayment.Commands.WithdrawalWebhook;
using Investment.Application.CryptoPayment.Queries.GetMinimumPaymentAmount;
using Investment.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Investment.Api.Controllers;

[ApiController]
[ApiRoute(1, "Payment/Crypto")]
public class CryptoPaymentController(IMediator mediator, ILogger<CryptoPaymentController> logger) : Controller
{
    #region Commads

    [Authorize]
    [HttpPost("Deposit")]
    public async Task<IActionResult> Deposit(DepositWithCryptoCommand command, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpPost("Deposit/Webhook/{userId}")]
    [AllowAnonymous]
    public async Task<IActionResult> WebhookDepositsStatus([FromRoute] int userId)
    {
        logger.LogWarning($"Enter WebhookDepositsStatus userId: {userId}.");

        var isHeaderValid = HttpContext.Request.Headers.TryGetValue("x-nowpayments-sig", out StringValues stringValues);

        logger.LogWarning($"isHeaderValid: {isHeaderValid}.");

        var providedSignature = stringValues.FirstOrDefault();

        if (!isHeaderValid || providedSignature is null || providedSignature.Length < 15)
        {
            logger.LogDebug("header:x-nowpayments-sig is required.");

            return NotFound();
        }

        HttpContext.Request.EnableBuffering();

        using var reader = new StreamReader(
            HttpContext.Request.Body,
            encoding: Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false, leaveOpen: true);

        var bodyRequest = await reader.ReadToEndAsync();

        HttpContext.Request.Body.Position = 0;

        if (string.IsNullOrEmpty(bodyRequest))
        {
            logger.LogWarning("body is required.");
            return NotFound();
        }

        logger.LogWarning($"deposit webhook called with bodyRequest: {bodyRequest}.");


        var hmacSha512WebhookVerifier = HttpContext.RequestServices.GetRequiredService<HmacSha512WebhookVerifier>();

        bool isWebhookVerify = hmacSha512WebhookVerifier.Verify(bodyRequest, providedSignature);

        if (!isWebhookVerify)
        {
            logger.LogWarning("Webhook Verify isn't Success.");
            return NotFound();
        }

        var command = JsonSerializer.Deserialize<DepositWebhookCommand>(bodyRequest);

        if (command is null)
        {
            logger.LogError(" Deserialize webhook is failed.");

            return NotFound();
        }

        command.UserId = userId;

        logger.LogWarning($"deposit webhook deserialized command: {JsonSerializer.Serialize(command)}.");

        await mediator.Send(command, CancellationToken.None);

        return Ok();
    }

    [Authorize]
    [HttpPost("Withdrawal")]
    public async Task<IActionResult> Withdrawal(WithdrawalCommand command, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(command, cancellationToken);

        return Ok(response);
    }



    [HttpPost("Withdrawal/Webhook/{userId}")]
    [AllowAnonymous]
    public async Task<IActionResult> WebhookWithdrawalStatus([FromRoute] int userId)
    {

        var isHeaderValid = HttpContext.Request.Headers.TryGetValue("x-nowpayments-sig", out StringValues stringValues);

        logger.LogWarning($"isHeaderValid: {isHeaderValid}.");

        var providedSignature = stringValues.FirstOrDefault();

        if (!isHeaderValid || providedSignature is null || providedSignature.Length < 15)
        {
            logger.LogDebug("header:x-nowpayments-sig is required.");

            return NotFound();
        }
        logger.LogDebug($"header:x-nowpayments-sig :{providedSignature} send");

        HttpContext.Request.EnableBuffering();

        using var reader = new StreamReader(
            HttpContext.Request.Body,
            encoding: Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false, leaveOpen: true);

        var bodyRequest = await reader.ReadToEndAsync();

        HttpContext.Request.Body.Position = 0;

        if (string.IsNullOrEmpty(bodyRequest))
        {
            logger.LogWarning("body is required.");
            return NotFound();
        }

        logger.LogWarning($"withdrawal webhook called with bodyRequest: {bodyRequest}.");

        var hmacSha512WebhookVerifier = HttpContext.RequestServices.GetRequiredService<HmacSha512WebhookVerifier>();

        bool isWebhookVerify = hmacSha512WebhookVerifier.Verify(bodyRequest, providedSignature);

        if (!isWebhookVerify)
        {
            logger.LogWarning("Webhook Verify isn't Success.");
            return NotFound();
        }

        var command = JsonSerializer.Deserialize<BatchWithdrawalWebhookCommand>(bodyRequest);

        if (command is null)
        {
            logger.LogError(" Deserialize webhook is failed.");

            return NotFound();
        }

        command.UserId = userId;

        await mediator.Send(command, CancellationToken.None);

        return Ok();
    }

    #endregion /Commadns

    [Authorize]
    [HttpGet("MinimumPaymentAmount")]
    public async Task<IActionResult> GetMinimumPaymentAmount(CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new MinimumPaymentAmountQuery(), cancellationToken);

        return Ok(response);
    }


    #region For Admin

    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
    [HttpPost("Withdrawal/Approve")]
    public async Task<IActionResult> WithdrawalApproveByAdmin(WithdrawalApproveByAdminCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);

        return Ok();
    }

    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
    [HttpPost("Withdrawal/SendPayment")]
    public async Task<IActionResult> WithdrawalSendPayment(WithdrawalSendPaymentByAdminCommand command, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(command, cancellationToken);

        return Ok(response);
    }

    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
    [HttpPost("Withdrawal/Verify")]
    public async Task<IActionResult> WithdrawalVerifyByAdmin(WithdrawalVerifyByAdminCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);

        return Ok();
    }

    #endregion /For Admin
}



