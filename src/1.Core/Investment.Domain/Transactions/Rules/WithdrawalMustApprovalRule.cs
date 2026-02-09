using Investment.Domain.Common;
using Investment.Domain.Transactions.Entities;
using Investment.Domain.Transactions.Enums.Transaction;

namespace Investment.Domain.Transactions.Rules;

internal class WithdrawalMustApprovalRule : IBusinessRule
{
    private Transaction transaction;

    public WithdrawalMustApprovalRule(Transaction transaction)
    {
        this.transaction = transaction;
    }

    public string Message => "withdrawal must in Approval status";

    public BusinessRuleCodeEnum BusinessRuleCode => BusinessRuleCodeEnum.WithdrawalMustApproval;

    public bool IsBroken() => transaction.TransactionStatus != nameof(WithdrawalStatusEnum.Approval);

}