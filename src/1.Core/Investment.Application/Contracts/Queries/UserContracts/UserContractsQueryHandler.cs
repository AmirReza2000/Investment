using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.QueryRepositories;
using MediatR;

namespace Investment.Application.Contracts.Queries.UserContracts;

public class UserContractsQueryHandler(
    IContractQueryRepository contractQueryRepository,
    IUserInfoService userInfoService)
    : IRequestHandler<UserContractsQuery, UserContractResponse[]>
{
    public async Task<UserContractResponse[]> Handle(UserContractsQuery request, CancellationToken cancellationToken)
    {
        return await contractQueryRepository.GetUserContractsForUserAsync(userId: userInfoService.GetUserId(), request, cancellationToken);
    }
}
