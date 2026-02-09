using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Infrastructure.Domain.Common;

public class QueryRepository
{
    protected readonly InvestmentDbContext dbContext;

    public QueryRepository(InvestmentDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
}
