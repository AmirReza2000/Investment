using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Investment.Application.Authenticate.VerifyUser;

public class VerifyUserCommand:IRequest
{
    public required string EmailAddress { get; set; }

    public required string Token { get; set; }
}
