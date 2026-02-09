using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Exceptions;
using Investment.Domain.Users;
using Investment.Domain.Users.Repositories;
using MediatR;

namespace Investment.Application.Authenticate.ChangePassword;

internal class ChangePasswordCommandHandler(
    IUserInfoService userInfoService,
    IUserCommandRepository userCommandRepository,
    IHashHelperService hashHelperService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ChangePasswordCommand>
{
    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userCommandRepository
             .GetUserByIdWithValidRefreshTokensAsync(userInfoService.GetUserId(), cancellationToken)
             ?? throw new InvalidCommandException("user not found", "user not found",ErrorCodeEnum.EntityNotFound);

        user.ChangePassword(
            currentPassword: request.CurrentPassword,
            newPassword: request.NewPassword,
            hashHelperService);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
