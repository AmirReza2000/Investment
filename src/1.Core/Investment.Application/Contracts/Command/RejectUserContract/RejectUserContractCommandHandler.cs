using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Exceptions;
using Investment.Domain.Contracts.Entities;
using Investment.Domain.Contracts.Repository;
using Investment.Domain.Transactions.Entities;
using Investment.Domain.Transactions.Enums.Transaction;
using Investment.Domain.Transactions.Repositories;
using MediatR;
using Microsoft.AspNetCore.Server.HttpSys;

namespace Investment.Application.Contracts.Command.RejectUserContract
{
    public class RejectUserContractCommandHandler(
    IContractCommandRepository contractCommandRepository,
    IUnitOfWork unitOfWork,
    IUserBalanceCommandRepository userBalanceCommandRepository) : IRequestHandler<RejectUserContractCommand>
    {
        public async Task Handle(RejectUserContractCommand request, CancellationToken cancellationToken)
        {
            var contract = await contractCommandRepository.GetByIdWithSpecificUserContractAsync(
                contractId: request.ContractId,
                userContractId: request.UserContractId,
                cancellationToken) ?? throw new EntityNotFoundException<Contract>();

            contract.RejectUserContract(request.UserContractId, request.Description, out int userId);

            UserBalance userBalance = await userBalanceCommandRepository.GetUserBalanceWithTransactionAsync(
                userId: userId,
                reason: request.UserContractId.ToString(),
                reasonType: ReasonTypeEnum.Contract,
                cancellationToken) ?? throw new EntityNotFoundException<UserBalance>();

            userBalance.CreateDepositForContractRejected(userContractId: request.UserContractId);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
