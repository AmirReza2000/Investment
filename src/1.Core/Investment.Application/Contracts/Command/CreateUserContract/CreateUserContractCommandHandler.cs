using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Contracts.Command.CreateUserContract;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Exceptions;
using Investment.Application.Utilities.QueryRepositories;
using Investment.Domain.Common;
using Investment.Domain.Contracts.Entities;
using Investment.Domain.Contracts.Repository;
using Investment.Domain.Transactions.Entities;
using Investment.Domain.Transactions.Repositories;
using MediatR;

namespace Investment.Application.Contracts.Command.CreateUserContract;

public class CreateUserContractCommandHandler(
    IContractCommandRepository contractCommandRepository,
    IContractQueryRepository contractQueryRepository,
    IUnitOfWork unitOfWork,
    IUserBalanceCommandRepository userBalanceCommandRepository,
    IUserInfoService userInfoService)
    : IRequestHandler<CreateUserContractCommand, int>
{
    public async Task<int> Handle(CreateUserContractCommand request, CancellationToken cancellationToken)
    {
        var contract = await contractCommandRepository.GetByIdAsync(request.ContractId, cancellationToken)
                    ?? throw new EntityNotFoundException<Contract>();

        ContractMarketType contractMarketType = await contractQueryRepository.GetContractMarketTypeAsync(contractMarketTypeId: request.ContractMarketTypeId, cancellationToken: cancellationToken)
            ?? throw new EntityNotFoundException<ContractMarketType>();

        int userId = userInfoService.GetUserId();

        var userContract = contract.CreateContractForUser(
            amount: request.Amount,
            durationOfDay: request.DurationOfDay,
            userId: userId,
            contractDurationType: request.ContractDurationType,
            contractMarketType);

        UserBalance userBalance = await userBalanceCommandRepository
            .GetUserBalanceByUserIdAsync(userId, cancellationToken)
                    ?? throw new EntityNotFoundException<UserBalance>();

        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);

            userBalance.CreateWithdrawalForContract(userContractId: userContract.Id, amount: userContract.Amount);

            await unitOfWork.SaveChangesAsync(cancellationToken);

        });

        return userContract.Id;

    }
}
