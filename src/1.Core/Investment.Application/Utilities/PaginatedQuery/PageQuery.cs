using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Utilities.PaginatedQuery;

public class PageQuery
{
    public int? PageNumber { get; set; }
    public int PageSize { get; set; } = 10;
}
