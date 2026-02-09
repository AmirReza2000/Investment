using Investment.Application.Utilities.Exceptions;
using Investment.Application.Utilities.QueryRepositories;
using Investment.Domain.Contracts.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Contracts.Queries.UserContractsForAdmin
{
    internal class UserContractsForAdminQueryHandler(
        IContractQueryRepository contractQueryRepository)
        : IRequestHandler<UserContractsForAdminQuery, UserContractsForAdminResponse[]>
    {
        public async Task<UserContractsForAdminResponse[]> Handle(UserContractsForAdminQuery request, CancellationToken cancellationToken)
        {
            return await contractQueryRepository.GetUserContractsForAdminAsync(userContractId: request.UserContractId, userId: request.UserId, cancellationToken)
                ?? throw new EntityNotFoundException<UserContract>();
        }
    }
}
