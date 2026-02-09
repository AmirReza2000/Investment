using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Constants;
using Investment.Domain.Contracts.Entities;
using Investment.Domain.Contracts.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using System.Text;

namespace Investment.Application.Contracts.Command.CreateContract;

public class CreateContractCommandHandler
    (IContractCommandRepository contractCommandRepository,
    IUnitOfWork unitOfWork,
    IObjectStoreService objectStoreService) :
    IRequestHandler<CreateContractCommand, int>
{
    public async Task<int> Handle(CreateContractCommand request, CancellationToken cancellationToken)
    {

        await contractCommandRepository.AssertContractTitleNotExistAsync(request.Title, cancellationToken);

        await contractCommandRepository.AssertContractImageNameNotExistAsync(request.ImageFile.FileName, cancellationToken);

        var newContract = Contract.Create(
            title: request.Title,
            imageName: request.ImageFile.FileName,
            rate: request.Rate,
            minDurationOfDay: request.MinDurationOfDay,
            minAmount: request.MinAmount,
            maxAmount: request.MaxAmount,
            0,
            metadata: request.Metadate);

        await objectStoreService.UploadAsync(request.ImageFile, ContractConstants.BucketNameImage);

        await contractCommandRepository.AddAsync(newContract, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return newContract.Id;
    }
}
