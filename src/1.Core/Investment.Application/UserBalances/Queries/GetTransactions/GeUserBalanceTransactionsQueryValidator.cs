using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Investment.Domain.Transactions.Enums.Transaction;

namespace Investment.Application.UserBalances.Queries.GetTransactions;

public class GeUserBalanceTransactionsQueryValidator : AbstractValidator<GeUserBalanceTransactionsQuery>
{
    public GeUserBalanceTransactionsQueryValidator()
    {
        RuleFor(x => x.TransactionTypeFilter)
                .IsInEnum();

        When(x => !string.IsNullOrWhiteSpace(x.TransactionStatusFilter), () =>
        {
            When(x => x.TransactionTypeFilter == TransactionTypeEnum.Withdrawal, () =>
            {
                RuleFor(x => x.TransactionStatusFilter)
                    .IsEnumName(typeof(WithdrawalStatusEnum), true)
                    .WithMessage("Invalid withdrawal status");
            });

            When(x => x.TransactionTypeFilter == TransactionTypeEnum.Deposit, () =>
            {
                RuleFor(x => x.TransactionStatusFilter)
                    .IsEnumName(typeof(DepositStatusEnum), true)
                    .WithMessage("Invalid deposit status");
            });
        });

        When(query => query.FromDateTime.HasValue, () =>
        {
            RuleFor(query => query.FromDateTime)
                .Must(from => from!.Value.Kind == DateTimeKind.Unspecified)
                .WithMessage("DateTime Kind must be Unspecified");

            RuleFor(query => query.FromDateTime)
           .LessThanOrEqualTo(query => query.ToDateTime)
           .When(query => query.ToDateTime.HasValue)
           .WithMessage("FromDateTime must be earlier than or equal to ToDateTime");
        });

        When(query => query.ToDateTime.HasValue, () =>
        {
            RuleFor(query => query.ToDateTime)
                .Must(from => from!.Value.Kind == DateTimeKind.Unspecified)
                .WithMessage("DateTime Kind must be Unspecified");
        });

    }
}
