using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Domain.Contracts.Enums;

public enum UserContractLogStatusEnum
{
    None=0,

    Create=10,

    Approve=20,

    Reject=30,

    Stop=40,

    ProfitDeposit =60,
}
