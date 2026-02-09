using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.Constants;
using Investment.Application.Utilities.QueryRepositories;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Investment.Application.Contracts.Queries.Contracts;

public class ContractsQueryHandler
    (IContractQueryRepository contractQueryRepository,
    IConfiguration configuration)
    : IRequestHandler<ContractsQuery, ContractsResponse[]>
{
    public async Task<ContractsResponse[]> Handle(ContractsQuery request, CancellationToken cancellationToken)
    {
        ContractsResponse[] responses = await contractQueryRepository.GetContractsPaginated(request, cancellationToken);

        string? objectStoreHostname = configuration["ObjectStoreConfig:Hostname"];

        foreach (var response in responses)
        {
            response.ImageUrl = "https://" + configuration["ObjectStoreConfig:Hostname"] + '/' + ContractConstants.BucketNameImage + '/' + response.ImageName;
        }

        return responses;
    }
}
