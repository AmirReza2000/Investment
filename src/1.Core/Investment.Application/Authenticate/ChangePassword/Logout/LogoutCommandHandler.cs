using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Exceptions;
using Investment.Domain.Users.Entities;
using Investment.Domain.Users.Repositories;
using MediatR;

namespace Investment.Application.Authenticate.ChangePassword.Logout;

public class LogoutCommandHandler(
    IUserInfoService userInfoService,
    IUserCommandRepository userCommandRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {

        int refreshTokenId = userInfoService.GetRefreshTokenId();

        User user =await  userCommandRepository.GetUserForLogoutAsync(
            userId: userInfoService.GetUserId(),
            refreshTokenId: refreshTokenId,
            cancellationToken)?? throw new EntityNotFoundException<User>();

        user.Logout(refreshTokenId: refreshTokenId);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
