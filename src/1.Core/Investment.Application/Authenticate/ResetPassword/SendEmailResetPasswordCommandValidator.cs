using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Investment.Application.Authenticate.ResetPassword;

public  class SendEmailResetPasswordCommandValidator:AbstractValidator<SendEmailResetPasswordCommand>
{
    public SendEmailResetPasswordCommandValidator()
    {
        RuleFor(command => command.EmailAddress)
            .EmailAddress();
    }
}
