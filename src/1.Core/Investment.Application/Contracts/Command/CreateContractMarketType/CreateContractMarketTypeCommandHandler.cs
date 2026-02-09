using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.Abstractions;
using Investment.Domain.Contracts.Entities;
using Investment.Domain.Contracts.Repository;
using MediatR;

namespace Investment.Application.Contracts.Command.CreateContractMarketType;

public class CreateContractMarketTypeCommandHandler(
    IContractCommandRepository contractCommandRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateContractMarketTypeCommand, int>
{
    public async Task<int> Handle(CreateContractMarketTypeCommand request, CancellationToken cancellationToken)
    {
        await contractCommandRepository.AssertContractMarketTypeNotExistAsync(request.Title, cancellationToken);

        ContractMarketType contractMarketType = new ContractMarketType(request.Title, request.Rate);

        await contractCommandRepository.AddMarketTypeContract(contractMarketType, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return contractMarketType.Id;
    }
}
