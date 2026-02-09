using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Investment.Application.Authenticate.ResendUserVerifyToken;

public class ResendUserVerifyTokenCommand:IRequest
{
    public required string EmailAddress { get; set; }
}
