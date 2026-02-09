using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Investment.Application.Authenticate.ResetPassword;

public class SendEmailResetPasswordCommand:IRequest
{
    public required string  EmailAddress { get; set; }
}
