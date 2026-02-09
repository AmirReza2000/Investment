using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Investment.Application.Utilities;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Configs;
using Investment.Application.Utilities.Constants;
using Investment.Application.Utilities.DTO.Nowpayment;
using Investment.Application.Utilities.Exceptions;
using Investment.Domain.Users.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Investment.Infrastructure.Services;

public class CryptoPaymentsService(
    CryptoPaymentsClientFactory cryptoPaymentsClientFactory,
    IOptions<CryptoPaymentConfig> cryptoPaymentConfigOptions,
    IConfiguration configuration) : ICryptoPaymentsService
{

    private readonly CryptoPaymentConfig cryptoPaymentConfig = cryptoPaymentConfigOptions.Value;

    private static readonly object _tokenSync = new();
    public Task<CreateCryptoDepositResponse> DepositRequestAsync(
        decimal priceAmount,
        int userId,
        CancellationToken cancellationToken)
    {

        CreateCryptoDepositRequest request = new CreateCryptoDepositRequest
        {
            PriceAmount = priceAmount.ToString(),
            PriceCurrency = "usd",
            PayCurrency = "usdttrc20",
            IpnCallbackUrl = configuration.GetValue<string>("BaseAddress") + "api/v1/Payment/Crypto/Deposit/Webhook/" + userId,
            IsFixedRate = true,
            IsFeePaidByUser = false
        };

        return cryptoPaymentsClientFactory.PostAsync<CreateCryptoDepositResponse>(
            CryptoPaymentConstants.CreateDepositPath,
            request,
           cancellationToken: cancellationToken);
    }

    public async Task AssertApiIsAvailableAsync(
        CancellationToken cancellationToken)
    {
        var response = await cryptoPaymentsClientFactory.GetAsync<CryptoApiStatusResponse>(
            CryptoPaymentConstants.ApiStatusPath,
            timeoutToSecond: 10,
            ct: cancellationToken);


        if (response.Message != "OK")
        {
            throw new InvalidCommandException("Payment gateway is not ready", "Payment gateway is not ready", ErrorCodeEnum.None);
        }
    }

    public async Task<CreateBatchCryptoWithdrawalResponse> WithdrawalRequestAsync(
        decimal amount,
        string address,
        string description,
        int userId,
        CancellationToken cancellationToken)
    {


        var request = new CreateBatchCryptoWithdrawalRequest
        {
            Withdrawals = new List<WithdrawalRequest>()
            {
                new WithdrawalRequest
                {
                    Address=address,
                    Amount="1",
                    Currency="usdttrc20",
                    FiatCurrency="usd",
                    FiatAmount=amount.ToString(),
                    IpnCallbackUrl = configuration.GetValue<string>("BaseAddress")+
                    "api/v1/Payment/Crypto/Withdrawal/Webhook/" + userId,
                    Description=description,
                }
            }
        };

        var batchCryptoWithdrawalResponse = await cryptoPaymentsClientFactory
            .PostWithJwtTokenAsync<CreateBatchCryptoWithdrawalResponse>(
            CryptoPaymentConstants.CreateWithdrawalPath,
             (await FetchTokenAsync(cancellationToken)).Token ?? throw new ArgumentNullException("jwtToken"),
            request,
             cancellationToken: cancellationToken
            );

        return batchCryptoWithdrawalResponse;
    }

    private Task<AuthenticationResponse> FetchTokenAsync(CancellationToken cancellationToken)
    {
        var request = new AuthenticationRequest
        {
            Email = cryptoPaymentConfig.EmailAddress,
            Password = cryptoPaymentConfig.Password
        };

        return cryptoPaymentsClientFactory.PostAsync<AuthenticationResponse>(
            CryptoPaymentConstants.AuthenticationPath,
            request,
             cancellationToken: cancellationToken
        );
    }

    public async Task VerifyWithdrawalAsync(string BatchWithdrawalId, string VerificationCode, CancellationToken cancellationToken)
    {
        await cryptoPaymentsClientFactory.PostWithJwtTokenAsync(
            CryptoPaymentConstants.VerifyWithdrawalPath.Replace("<batch-withdrawal-id>", BatchWithdrawalId.ToString()),
         (await FetchTokenAsync(cancellationToken)).Token ?? throw new ArgumentNullException("jwtToken"),
            new { verification_code = VerificationCode },
            cancellationToken: cancellationToken
        );
    }

    public async Task<MinimumPaymentAmountResponse> GetMinimumPaymentAmountAsync(CancellationToken cancellationToken)
    {
        return await cryptoPaymentsClientFactory.GetAsync<MinimumPaymentAmountResponse>(
             CryptoPaymentConstants.MinimumPaymentAmountPath,
             timeoutToSecond: 10,
             ct: cancellationToken);
    }
}
