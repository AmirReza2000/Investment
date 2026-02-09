using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using System.Transactions;
using Investment.Domain.Common;
using Investment.Domain.Common.Exceptions;
using Investment.Domain.Transactions.Enums.Transaction;
using Investment.Domain.Transactions.Rules;

namespace Investment.Domain.Transactions.Entities;

public class UserBalance : AggregateRoot<int>
{
    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public uint Version { get; private set; }

    private List<Transaction> _transactions = [];

    public IReadOnlyList<Transaction> Transactions = [];

    private UserBalance()
    {
        Transactions = _transactions;
    }

    private UserBalance(int userId)
    {
        UserId = userId;
    }

    public static UserBalance Create(int userId)
    {
        return new UserBalance(userId);
    }

    public Transaction CreateDeposit(decimal amount, string paymentId, string paymentAddress)
    {
        Transaction transaction = Transaction.Create(
            amount: amount,
            reason: paymentId,
            ReasonTypeEnum.NowPayment,
            TransactionTypeEnum.Deposit,
            address: paymentAddress);

        _transactions.Add(transaction);

        return transaction;
    }

    public Transaction CreateWithdrawal(decimal amountWithdrawal, string addressWithdrawal)
    {
        CheckRule(new WalletAdequacyForWithdrawalRule(
            userBalance: Amount,
            amountWithdrawal: amountWithdrawal));

        Transaction transaction = Transaction.Create(
            amount: amountWithdrawal,
            reason: string.Empty,
            ReasonTypeEnum.NowPayment,
            TransactionTypeEnum.Withdrawal,
            //userBalanceId: Id,
            address: addressWithdrawal);

        _transactions.Add(transaction);

        Amount -= amountWithdrawal;

        return transaction;
    }


    public void UpdateDepositStatus(DepositStatusEnum depositStatusEnum, long paymentId, string payAddress)
    {
        var transaction = _transactions
            .FirstOrDefault(t => t.Reason == paymentId.ToString() && t.Address == payAddress)
            ?? throw new NullReferenceException($"transaction with reason:{paymentId} not found.");


        if (depositStatusEnum == DepositStatusEnum.finished)
        {
            if (transaction.TransactionStatus != DepositStatusEnum.finished.ToString())
                Amount += transaction.Amount;
        }

        CheckRule(new TransactionMustBeDepositTypeRule(transaction.TransactionType));

        if (transaction.TransactionStatus == depositStatusEnum.ToString()) return;

        transaction.UpdateDepositStatus(depositStatusEnum);
    }

    public void ApproveWithdrawalTransaction(int transactionId)
    {
        var transaction = Transactions.FirstOrDefault(
            transaction => transaction.Id == transactionId);

        if (transaction is null)
            CheckRule(new GeneralBusinessRule($"Transaction with id:{transactionId} not found.", BusinessRuleCodeEnum.EntityNotFound));

        transaction!.UpdateWithdrawal(WithdrawalStatusEnum.Approval);
    }

    public Transaction SendWithdrawalPayment(int transactionId)
    {
        Transaction? transaction = Transactions.FirstOrDefault(transaction => transaction.Id == transactionId);

        if (transaction is null)
            CheckRule(new GeneralBusinessRule(
                $"Transaction with id:{transactionId} not found.", BusinessRuleCodeEnum.EntityNotFound));

        CheckRule(new WithdrawalMustApprovalRule(transaction!));

        transaction!.UpdateWithdrawal(WithdrawalStatusEnum.Creating);

        return transaction;
    }

    public void AssignReasonForWithdrawal(int id, string reason)
    {
        var transaction = Transactions.Single(entity => entity.Id == id);

        transaction.AssignReason(reason);

        transaction.UpdateWithdrawal(WithdrawalStatusEnum.Waiting);
    }

    public Transaction VerifyWithdrawalTransaction(int transactionId)
    {
        Transaction? transaction = Transactions.FirstOrDefault(transaction => transaction.Id == transactionId);

        if (transaction is null)
            CheckRule(new GeneralBusinessRule(
                $"Transaction with id:{transactionId} not found.", BusinessRuleCodeEnum.EntityNotFound));

        CheckRule(new WithdrawalMustCreatingRule(transaction!));

        transaction!.UpdateWithdrawal(WithdrawalStatusEnum.Waiting);

        return transaction;
    }

    public void UpdateWithdrawalStatus(WithdrawalStatusEnum withdrawalStatusEnum, string reason)
    {
        var transaction = _transactions
                   .FirstOrDefault(t => t.Reason == reason)
                   ?? throw new Exception($"transaction with reason:{reason} not found.");

        transaction.UpdateWithdrawal(withdrawalStatusEnum);
    }

    public void CreateWithdrawalForContract(int userContractId, decimal amount)
    {
        CheckRule(new WalletAdequacyForWithdrawalRule(
           userBalance: Amount,
           amountWithdrawal: amount));

        var transaction = Transaction.CreateContractTransaction(
            amount: amount,
            reason: userContractId.ToString(),
            transactionType: TransactionTypeEnum.Withdrawal);

        _transactions.Add(transaction);

        Amount -= amount;
    }

    public void CreateDepositForContractRejected(int userContractId)
    {
        var contractWithdrawalTransaction = _transactions.SingleOrDefault(transaction =>
        transaction.Reason == userContractId.ToString() &&
        transaction.ReasonType == ReasonTypeEnum.Contract &&
        transaction.TransactionType == TransactionTypeEnum.Withdrawal)
            ?? throw new NullReferenceException("No deposit transaction has been recorded for the contract.");

        var transactionContractRejected = Transaction.CreateContractTransaction(
            amount: contractWithdrawalTransaction.Amount,
            reason: userContractId.ToString(),
            transactionType: TransactionTypeEnum.Deposit);

        _transactions.Add(transactionContractRejected);

        Amount += contractWithdrawalTransaction.Amount;
    }

    public void CreateDepositForContractProfit(int userContractId, int userContractProfitId, decimal profit)
    {
        Transaction newTransaction = Transaction.CreateContractProfitTransaction(
             userContractId: userContractId,
             userContractProfitId: userContractProfitId,
             profit: profit);

        _transactions.Add(newTransaction);

        Amount += profit;
    }
}

