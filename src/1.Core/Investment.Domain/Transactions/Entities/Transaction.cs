using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using System.Transactions;
using Investment.Domain.Common;
using Investment.Domain.Transactions.Enums.Transaction;
using Investment.Domain.Transactions.Rules;

namespace Investment.Domain.Transactions.Entities;

public class Transaction : Entity<int>
{
    public decimal Amount { get; private set; }

    public string Reason { get; private set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public ReasonTypeEnum ReasonType { get; private set; }

    public string TransactionStatus { get; private set; } = string.Empty;

    public TransactionTypeEnum TransactionType { get; private set; }

    public DateTime CreateDatetime { get; private set; }

    public int UserBalanceId { get; private set; }
    public UserBalance? UserBalance { get; private set; }

    private Transaction()
    {

    }

    private Transaction(decimal amount,
        string reason,
        ReasonTypeEnum reasonType,
        TransactionTypeEnum transactionType,
        string transactionStatus,
        string address)
    {
        Amount = amount;
        Reason = reason;
        ReasonType = reasonType;
        CreateDatetime = DateTime.Now;
        TransactionType = transactionType;
        TransactionStatus = transactionStatus;
        Address = address;
    }

    internal static Transaction Create(
        decimal amount,
        string reason,
        ReasonTypeEnum reasonType,
        TransactionTypeEnum transactionType,
        string address)
    {
        return new Transaction(amount,
            reason,
            reasonType,
            transactionType,
            transactionStatus: transactionType ==
            TransactionTypeEnum.Deposit ?
            DepositStatusEnum.waiting.ToString() :
            WithdrawalStatusEnum.AwaitingApproval.ToString(),
            address);
    }

    internal static Transaction CreateContractTransaction(
        decimal amount,
        string reason,
        TransactionTypeEnum transactionType)
    {
        return new Transaction(
           amount,
           reason,
           ReasonTypeEnum.Contract,
           transactionType,
           transactionStatus: transactionType ==
           TransactionTypeEnum.Deposit ?
           DepositStatusEnum.finished.ToString() :
           WithdrawalStatusEnum.Finished.ToString(),
           "");
    }

    internal static Transaction CreateContractProfitTransaction(int userContractId, int userContractProfitId, decimal profit)
    {
        return new Transaction(
                 amount: profit,
                  reason: userContractId.ToString(),
                  ReasonTypeEnum.ContractProfit,
                  TransactionTypeEnum.Deposit,
                  transactionStatus: DepositStatusEnum.finished.ToString(),
                  address: userContractProfitId.ToString());
    }
    internal void UpdateDepositStatus(DepositStatusEnum depositStatus)
    {

        TransactionStatus = depositStatus.ToString();

    }

    internal void ApproveWithdrawal()
    {

        TransactionStatus = WithdrawalStatusEnum.Approval.ToString();
    }

    internal void UpdateWithdrawal(WithdrawalStatusEnum status)
    {
        CheckRule(new TransactionMustBeWithdrawalTypeRule(TransactionType));

        if (TransactionStatus == status.ToString()) return;

        TransactionStatus = status.ToString();
    }

    internal void AssignReason(string reason)
    {
        Reason = reason;
    }


}
