using Investment.Domain.Common;
using Investment.Domain.Transactions.Enums.Transaction;

namespace Investment.Domain.Transactions.Rules
{
    internal class TransactionMustBeWithdrawalTypeRule : IBusinessRule
    {
        private TransactionTypeEnum transactionType;

        public TransactionMustBeWithdrawalTypeRule(TransactionTypeEnum transactionType)
        {
            this.transactionType = transactionType;
        }

        public string Message => "transaction is not found.";

        public BusinessRuleCodeEnum BusinessRuleCode => BusinessRuleCodeEnum.TransactionMustBeWithdrawalType;

        public bool IsBroken() => transactionType != TransactionTypeEnum.Withdrawal;
    }
}