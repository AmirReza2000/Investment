using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.Abstractions;
using Investment.Domain.Common.Exceptions;
using Investment.Domain.Transactions.Entities;
using Investment.Domain.Transactions.Repositories;
using Investment.Domain.Users;
using Investment.Domain.Users.Repositories;
using Investment.Domain.Users.Rules;
using MediatR;

namespace Investment.Application.Authenticate.VerifyUser;

public class VerifyUserCommandHandler (
    IUserCommandRepository userCommandRepository,
    IUserBalanceCommandRepository userBalanceCommandRepository,
    IUnitOfWork unitOfWork,
    IHashHelperService hashHelperService)
    : IRequestHandler<VerifyUserCommand>
{
    public async Task Handle(VerifyUserCommand request, CancellationToken cancellationToken)
    {
        var hashedVerifyToken =
          hashHelperService.HashVerificationToken(request.Token);

        var user=await userCommandRepository.GetUserWithSpecifiedValidationTokenAsync(
          emailAddress: request.EmailAddress,
          hashedVerifyToken: hashedVerifyToken,
            cancellationToken);

        if (user == null)
            throw new BusinessRuleValidationException(
               new ValidVerificationTokenRule(null!, null!));


        user.Verify(hashedVerifyToken);

        await userBalanceCommandRepository.AddAsync(UserBalance.Create(user.Id), default);

        await unitOfWork.SaveChangesAsync(cancellationToken); 
    }
}
