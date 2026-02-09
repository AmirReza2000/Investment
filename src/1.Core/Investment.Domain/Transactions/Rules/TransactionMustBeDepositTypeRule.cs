using Investment.Domain.Common;
using Investment.Domain.Transactions.Enums.Transaction;

namespace Investment.Domain.Transactions.Rules;

internal class TransactionMustBeDepositTypeRule : IBusinessRule
{
    private TransactionTypeEnum transactionType;

    public TransactionMustBeDepositTypeRule(TransactionTypeEnum transactionType)
    {
        this.transactionType = transactionType;
    }

    public string Message => "transaction is not found.";

    public BusinessRuleCodeEnum BusinessRuleCode => BusinessRuleCodeEnum.TransactionMustBeDepositType;

    public bool IsBroken() => transactionType != TransactionTypeEnum.Deposit;
}