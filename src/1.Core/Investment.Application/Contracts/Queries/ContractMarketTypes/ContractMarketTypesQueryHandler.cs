using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.QueryRepositories;
using MediatR;

namespace Investment.Application.Contracts.Queries.ContractMarketTypes
{
    internal class ContractMarketTypesQueryHandler(
        IContractQueryRepository contractQueryRepository) :
        IRequestHandler<ContractMarketTypesQuery, ContractMarketTypeResponse[]>
    {
        public async Task<ContractMarketTypeResponse[]> Handle(ContractMarketTypesQuery request, CancellationToken cancellationToken)
        {
          return await contractQueryRepository.GetContractMarketTypesAsync(request, cancellationToken);
        }
    }
}
