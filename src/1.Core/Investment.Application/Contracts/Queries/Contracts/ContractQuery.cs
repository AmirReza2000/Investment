using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Contracts.Queries.Contracts;

public class ContractQuery : IRequest<ContractResponse>
{
    public required int ContractId { get; set; }
}
