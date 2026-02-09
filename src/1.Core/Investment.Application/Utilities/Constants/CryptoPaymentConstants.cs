using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Utilities.Constants;

public static class CryptoPaymentConstants
{
    private const string ApiVersion = "v1";

    public const string CreateDepositPath = $"{ApiVersion}/payment";

    public const string AuthenticationPath = $"{ApiVersion}/auth";

    public const string ApiStatusPath = $"{ApiVersion}/status";

    public const string CreateWithdrawalPath = $"{ApiVersion}/payout";

    public const string VerifyWithdrawalPath = $"{ApiVersion}/payout/<batch-withdrawal-id>/verify";

    public const string MinimumPaymentAmountPath = $"{ApiVersion}/min-amount?currency_from=usd&currency_to=usdttrc20&fiat_equivalent=usd&is_fixed_rate=True&is_fee_paid_by_user=True";
}
