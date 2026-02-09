using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.Exceptions;
using Investment.Domain.Users;
using Investment.Domain.Users.Repositories;
using MediatR;

namespace Investment.Application.Authenticate.ResetPassword;

public class VerifyTokenResetPasswordCommandHandler (
    IUserCommandRepository userCommandRepository,
    IHashHelperService  hashHelperService
    ): IRequestHandler<VerifyTokenResetPasswordCommand>
{
    public async Task Handle(VerifyTokenResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var hashedToken=hashHelperService.HashVerificationToken(request.Token);

        bool isValid=await userCommandRepository.IsTokenResetPasswordValidAsync(
            request.EmailAddress, 
            hashedToken,
            cancellationToken);

        if (!isValid)
            throw new InvalidCommandException("Invalid Token.", "Invalid Token.",ErrorCodeEnum.None);
    }
}
