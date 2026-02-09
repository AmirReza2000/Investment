using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Domain.Users.Enums.User
{
    public enum UserStatusEnum
    {
        Active = 0,
        ChangeProfilePending = 1,
        Pending = 2,
        NotVerified = 3,
        Ban = 4,
        Delete = 5,
    }
}
