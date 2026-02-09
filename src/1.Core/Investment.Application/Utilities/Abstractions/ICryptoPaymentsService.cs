using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.DTO.Nowpayment;

namespace Investment.Application.Utilities.Abstractions;

public interface ICryptoPaymentsService
{
    Task<CreateCryptoDepositResponse> DepositRequestAsync(
        decimal amount,
        int userId,
        CancellationToken cancellationToken);
    Task AssertApiIsAvailableAsync(
        CancellationToken cancellationToken);
    Task<CreateBatchCryptoWithdrawalResponse> WithdrawalRequestAsync(
        decimal amount,
        string address,
        string description,
        int userId,
        CancellationToken cancellationToken);
    Task VerifyWithdrawalAsync(string BatchWithdrawalId, string VerificationCode, CancellationToken cancellationToken);
    Task<MinimumPaymentAmountResponse> GetMinimumPaymentAmountAsync(CancellationToken cancellationToken);
}