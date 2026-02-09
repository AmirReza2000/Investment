using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Investment.Domain.Contracts.Enums;
using MediatR;

namespace Investment.Application.Contracts.Queries.ExpectedProfit;

public class ExpectedProfitQuery : IRequest<ExpectedProfitResponse>
{
    public required int ContractId { get; set; }

    public required int Amount { get; set; }

    public required int DurationOfDay { get; set; }

    public required int MarketTypeId { get; set; }

    public required ContractDurationTypeEnum ContractDurationType { get; set; }
}
