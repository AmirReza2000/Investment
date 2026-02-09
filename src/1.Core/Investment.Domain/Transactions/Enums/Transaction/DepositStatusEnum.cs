using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Domain.Transactions.Enums.Transaction;

public enum DepositStatusEnum : int
{
    None = 0,

    waiting = 10,

    confirming = 20,

    confirmed = 30,

    sending = 40,

    partially_paid = 50,

    finished = 60,

    failed = 70,

    refunded = 80,

    expired = 90,
}
