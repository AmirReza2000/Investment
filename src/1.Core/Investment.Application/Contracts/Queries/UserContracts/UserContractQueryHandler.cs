using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Exceptions;
using Investment.Application.Utilities.QueryRepositories;
using Investment.Domain.Contracts.Entities;
using MediatR;

namespace Investment.Application.Contracts.Queries.UserContracts;

public class UserContractQueryHandler(IContractQueryRepository contractQueryRepository,
    IUserInfoService userInfoService)
    : IRequestHandler<UserContractQuery, UserContractResponse>
{
    public async Task<UserContractResponse> Handle(UserContractQuery request, CancellationToken cancellationToken)
    {
        return await contractQueryRepository.GetUserContractForUserAsync(
            userId: userInfoService.GetUserId(), request.UserContractId, cancellationToken)
                ?? throw new EntityNotFoundException<UserContract>();
    }
}
