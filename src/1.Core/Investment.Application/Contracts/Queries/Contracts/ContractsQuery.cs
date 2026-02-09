using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Investment.Application.Utilities.PaginatedQuery;
using MediatR;

namespace Investment.Application.Contracts.Queries.Contracts;

public class ContractsQuery : PageQuery, IRequest<ContractsResponse[]>
{
    public bool? ActiveFilter { get; set; }
}
