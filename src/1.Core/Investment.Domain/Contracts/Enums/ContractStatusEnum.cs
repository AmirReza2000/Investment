using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Domain.Contracts.Enums;

public enum ContractStatusEnum
{
    None=0,
    PendingApprove=10,
    Approved=20,
    Rejected=30,
    Stopped = 40,
}
