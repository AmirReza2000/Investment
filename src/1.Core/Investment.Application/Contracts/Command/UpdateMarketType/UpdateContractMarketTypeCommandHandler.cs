using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Exceptions;
using Investment.Domain.Contracts.Entities;
using Investment.Domain.Contracts.Repository;
using MediatR;

namespace Investment.Application.Contracts.Command.UpdateMarketType;

public class UpdateContractMarketTypeCommandHandler(
       IContractCommandRepository contractCommandRepository,
       IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateContractMarketTypeCommand>
{
    public async Task Handle(UpdateContractMarketTypeCommand request, CancellationToken cancellationToken)
    {
        ContractMarketType contractMarketType =
            await contractCommandRepository.GetContractMarketTypeById(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException<ContractMarketType>();

        if (contractMarketType.Title != request.Title) await contractCommandRepository.AssertContractMarketTypeNotExistAsync(request.Title, cancellationToken);

        contractMarketType.Update(request.Title, request.IsActive, request.Rate);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
