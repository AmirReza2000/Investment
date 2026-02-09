using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Exceptions;
using Investment.Domain.Common.Exceptions;
using Investment.Domain.Users;
using Investment.Domain.Users.Entities;
using Investment.Domain.Users.Repositories;
using Investment.Domain.Users.Rules;
using MediatR;

namespace Investment.Application.Authenticate.ResetPassword;

public class ResetPasswordCommandHandler(
    IUserCommandRepository userCommandRepository,
    IUnitOfWork unitOfWork,
    IHashHelperService hashHelperService
    ) : IRequestHandler<ResetPasswordCommand>
{
    public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var hashedToken = hashHelperService.HashVerificationToken(request.Token);

        var user = await userCommandRepository.
            GetUserForResetPasswordAsync(
            request.EmailAddress,
            hashedToken,
            cancellationToken);

        if (user == null)
            throw new BusinessRuleValidationException(
               new ValidVerificationTokenRule(null!, null!));

        user.RestPassword(request.NewPassword, hashedToken, hashHelperService);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
