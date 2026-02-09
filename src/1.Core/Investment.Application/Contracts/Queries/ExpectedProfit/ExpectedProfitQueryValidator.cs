using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Investment.Application.Contracts.Queries.ExpectedProfit;

public class ExpectedProfitQueryValidator:AbstractValidator<ExpectedProfitQuery>

{
    public ExpectedProfitQueryValidator()
    {
        RuleFor(RuleFor => RuleFor.Amount)
            .GreaterThan(0);

        RuleFor(RuleFor => RuleFor.DurationOfDay)
            .GreaterThanOrEqualTo(30);

        RuleFor(RuleFor => RuleFor.MarketTypeId)
            .NotEmpty();

        RuleFor(RuleFor => RuleFor.ContractDurationType)
            .NotEmpty()
            .IsInEnum();
    }
}
