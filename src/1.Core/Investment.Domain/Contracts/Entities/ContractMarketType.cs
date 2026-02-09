using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Common;

namespace Investment.Domain.Contracts.Entities;

public class ContractMarketType : Entity<int>
{
    public string Title { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    public decimal Rate { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    private ContractMarketType()
    {

    }

    public ContractMarketType(string title, decimal rate)
    {
        Title = title;
        IsActive = false;
        Rate = rate;
        CreatedAt = DateTime.Now;
    }

    public void Update(string title, bool isActive, decimal rate)
    {
        IsActive = isActive;
        Rate = rate;
        Title = title;
        UpdatedAt = DateTime.Now;
    }
}
