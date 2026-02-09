using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Authenticate.RefreshToken;

public class RefreshTokenCommand : IRequest<RefreshTokenResponse?>
{
    public required string RefreshToken { get; set; }
}
