using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Investment.Application.Contracts.Queries.UserContracts;

public class UserContractQuery:IRequest<UserContractResponse>
{
    public required int UserContractId { get; set; }
}
