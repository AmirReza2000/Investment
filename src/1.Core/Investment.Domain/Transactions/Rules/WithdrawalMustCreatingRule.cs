using Investment.Domain.Common;
using Investment.Domain.Transactions.Entities;
using Investment.Domain.Transactions.Enums.Transaction;

namespace Investment.Domain.Transactions.Rules;

internal class WithdrawalMustCreatingRule : IBusinessRule
{
    private Transaction transaction;

    public WithdrawalMustCreatingRule(Transaction transaction)
    {
        this.transaction = transaction;
    }

    public string Message => "withdrawal must in Creating status";

    public BusinessRuleCodeEnum BusinessRuleCode => BusinessRuleCodeEnum.WithdrawalMustInCreatingStatus;

    public bool IsBroken() => transaction.TransactionStatus != nameof(WithdrawalStatusEnum.Creating);

}