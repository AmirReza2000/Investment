using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Domain.Transactions.Enums.Transaction;

public enum WithdrawalStatusEnum : int
{
    None = 0,

    AwaitingApproval = 10,

    Approval = 20,

    Creating = 30,

    Waiting = 40,

    WaitingForProcessing = 45,

    Processing = 50,

    Sending = 60,

    Finished = 70,

    Failed = 80,

    Rejected = 90,

}
