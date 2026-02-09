using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Contracts.Entities;
using Investment.Domain.Contracts.Enums;
using MediatR;

namespace Investment.Application.Contracts.Command.CreateUserContract;

public class CreateUserContractCommand : IRequest<int>
{
    public required decimal Amount { get; set; }

    public required int DurationOfDay { get; set; }

    public required ContractDurationTypeEnum ContractDurationType { get; set; }

    public required int ContractMarketTypeId { get; set; }

    public required int ContractId { get; set; }
}
