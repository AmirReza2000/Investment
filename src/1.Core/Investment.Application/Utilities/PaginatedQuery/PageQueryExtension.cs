using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Utilities.PaginatedQuery;

public static class PageQueryExtension
{
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, PageQuery paginationQuery)
    {

        if (paginationQuery.PageNumber is null)
        {
            return query;
        }

        int skipCount = (int)((paginationQuery.PageNumber - 1) * paginationQuery.PageSize);

        return query.Skip(skipCount).Take(paginationQuery.PageSize);
    }
}