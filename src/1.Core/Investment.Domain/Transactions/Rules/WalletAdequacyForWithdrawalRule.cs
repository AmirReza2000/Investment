using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Common;

namespace Investment.Domain.Transactions.Rules;

public class WalletAdequacyForWithdrawalRule : IBusinessRule
{
    private readonly decimal userBalance;
    private readonly decimal amountWithdrawal;

    public string Message => "Insufficient wallet balance.";

    public BusinessRuleCodeEnum BusinessRuleCode => BusinessRuleCodeEnum.WalletAdequacy;

    public WalletAdequacyForWithdrawalRule(decimal userBalance,decimal amountWithdrawal)
    {
        this.userBalance = userBalance;
        this.amountWithdrawal = amountWithdrawal;
    }

    public bool IsBroken()=>userBalance<amountWithdrawal;

}
