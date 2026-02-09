using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Investment.Application.Contracts.Command.ApproveUserContract;

public class ApproveUserContractCommand:IRequest
{
    public required int ContractId { get; set; }

    public required int UserContractId { get; set; }
}
