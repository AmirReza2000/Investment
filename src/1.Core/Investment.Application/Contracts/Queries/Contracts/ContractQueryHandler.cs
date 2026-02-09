using Investment.Application.Utilities.Constants;
using Investment.Application.Utilities.Exceptions;
using Investment.Application.Utilities.QueryRepositories;
using Investment.Domain.Contracts.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Contracts.Queries.Contracts;

public class ContractQueryHandler(
    IContractQueryRepository contractQueryRepository,
    IConfiguration configuration)
    : IRequestHandler<ContractQuery, ContractResponse>
{
    public async Task<ContractResponse> Handle(ContractQuery query, CancellationToken cancellationToken)
    {
        ContractResponse response = await contractQueryRepository.GetContractByIdAsync(Id: query.ContractId, cancellationToken)
            ?? throw new EntityNotFoundException<Contract>();

        response.ImageUrl = "https://" + configuration["ObjectStoreConfig:Hostname"] + '/' + ContractConstants.BucketNameImage + '/' + response.ImageName;

        return response;
    }
}