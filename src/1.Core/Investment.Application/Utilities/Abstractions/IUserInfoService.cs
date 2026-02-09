using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Utilities.Abstractions;

public interface IUserInfoService
{
    int GetUserId();

    int GetRefreshTokenId();
}
