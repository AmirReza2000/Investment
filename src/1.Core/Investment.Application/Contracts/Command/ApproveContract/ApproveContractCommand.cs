using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Investment.Application.Contracts.Command.ApproveContract;

public class ApproveContractCommand:IRequest
{
    public required int ContractId { get; set; }

    public required int UserContractId { get; set; }
}
