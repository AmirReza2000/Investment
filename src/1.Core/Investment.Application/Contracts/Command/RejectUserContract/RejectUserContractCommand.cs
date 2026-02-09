using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Investment.Application.Contracts.Command.RejectUserContract;

public class RejectUserContractCommand:IRequest
{
    public required int ContractId { get; set; }

    public required int UserContractId { get; set; }

    public required string Description { get; set; }
}
