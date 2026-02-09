using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Exceptions;
using Investment.Domain.Common;
using Investment.Domain.Contracts.Entities;
using Investment.Domain.Contracts.Repository;
using MediatR;

namespace Investment.Application.Contracts.Command.UpdateContract;

public class UpdateContractCommandHandler(
    IContractCommandRepository contractCommandRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateContractCommand>
{
    public async Task Handle(UpdateContractCommand request, CancellationToken cancellationToken)
    {
        var contract = await contractCommandRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException<Contract>();

        if (request.Title != contract.Title) await contractCommandRepository.AssertContractTitleNotExistAsync(request.Title, cancellationToken);

        contract.Update(request.Title, request.Rate, request.MinAmount, request.MaxAmount, request.MinDurationOfDay, request.ImageName, request.IsActive,request.Metadate);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
