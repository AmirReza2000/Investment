using Investment.Domain.Users.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Domain.Common;

public interface ITokenAuthenticateService
{
    string GenerateJwtToken(string emailAddress, string userId, string refreshTokenId);
    string GenerateRefreshToken();
    DateTime GetRefreshTokenExpireAt();
}
