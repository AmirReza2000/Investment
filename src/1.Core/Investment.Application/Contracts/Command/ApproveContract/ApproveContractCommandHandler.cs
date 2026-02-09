using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Exceptions;
using Investment.Domain.Contracts.Entities;
using Investment.Domain.Contracts.Repository;
using MediatR;

namespace Investment.Application.Contracts.Command.ApproveContract;

public class ApproveContractCommandHandler(
    IContractCommandRepository contractCommandRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ApproveContractCommand>
{
    public async Task Handle(ApproveContractCommand request, CancellationToken cancellationToken)
    {
        Contract contract=await contractCommandRepository.GetByIdWithSpecificUserContractAsync(
            contractId: request.ContractId,
            userContractId:request.UserContractId,cancellationToken)
            ?? throw new EntityNotFoundException<Contract>();

        contract.ApproveUserContract(userContractId: request.UserContractId);

        await unitOfWork.SaveChangesAsync(cancellationToken);  
    }
}
