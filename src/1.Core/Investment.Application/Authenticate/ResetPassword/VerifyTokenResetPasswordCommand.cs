using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Investment.Application.Authenticate.ResetPassword;

public class VerifyTokenResetPasswordCommand:IRequest
{
    public required string EmailAddress { get; set; }

    public required string Token { get; set; }
}
