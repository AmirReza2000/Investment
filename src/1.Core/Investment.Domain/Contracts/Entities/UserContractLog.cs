using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Common;
using Investment.Domain.Contracts.Enums;

namespace Investment.Domain.Contracts.Entities;

public class UserContractLog : Entity<int>
{
    public string Description { get; private set; } = string.Empty;

    public UserContractLogStatusEnum Status { get;private set; }

    public DateTime CreateDateTime { get; private set; }

    public int UserContractId { get; private set; }
    public UserContract? UserContract { get; private set; }

    private UserContractLog()
    {
        
    }

    internal UserContractLog(string description, 
        UserContractLogStatusEnum status)
    {
        Description = description;
        Status = status;
        CreateDateTime= DateTime.Now;
    }
}
