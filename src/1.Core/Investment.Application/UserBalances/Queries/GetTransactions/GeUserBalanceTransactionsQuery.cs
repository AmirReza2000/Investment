using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.PaginatedQuery;
using Investment.Domain.Transactions.Enums.Transaction;
using MediatR;

namespace Investment.Application.UserBalances.Queries.GetTransactions;

public class GeUserBalanceTransactionsQuery: PageQuery, IRequest<TransactionResponse[]>
{
  public TransactionTypeEnum TransactionTypeFilter { get; set; }

  public string?  TransactionStatusFilter { get; set; }

  public DateTime? FromDateTime { get; set; }
  
  public DateTime? ToDateTime { get; set; }
} 
