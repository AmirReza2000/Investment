using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Transactions.Enums.Transaction;

namespace Investment.Application.UserBalances.Queries.GetTransactions;

public class TransactionResponse
{

    public decimal Amount { get; set; }

    public required string Reason { get; set; }

    public required string Address { get; set; }

    public ReasonTypeEnum ReasonType { get; set; }

    public required string TransactionStatus { get; set; }

    public TransactionTypeEnum TransactionType { get; set; }

    public DateTime CreateDatetime { get; set; }
    public int Id { get; set; }
}

