using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Investment.Application.Utilities.PaginatedQuery;
using Investment.Domain.Contracts.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Investment.Application.Contracts.Queries.UserContracts;

public class UserContractsQuery : PageQuery, IRequest<UserContractResponse[]>
{
    public  string? TitleMarketTypeFilter  { get; set; }

    public string? TitleContractFilter { get; set; }

    public UserContractStatusEnum? StatusFilter { get; set; }

    public DateTime? AcceptAtDateTimeFilter { get; set; }
}
