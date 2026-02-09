using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Investment.Application.Authenticate.ChangePassword;

public class ChangePasswordCommand:IRequest
{
    public required string CurrentPassword { get; set; }

    public required string NewPassword { get; set; }

    public required string RepeatNewPassword { get; set; }
}
