using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Common;
using Investment.Domain.Contracts.Entities;

namespace Investment.Domain.Contracts.Repository;

public interface IContractCommandRepository : ICommandRepository<Contract, int>
{
    Task AddMarketTypeContract(ContractMarketType contractMarketType, CancellationToken cancellationToken);
    Task AssertContractImageNameNotExistAsync(string ImageName, CancellationToken cancellationToken);
    Task AssertContractMarketTypeNotExistAsync(string title, CancellationToken cancellationToken);
    Task AssertContractTitleNotExistAsync(string title, CancellationToken cancellationToken);
    Task<Contract?> GetByIdWithSpecificUserContractAsync(int contractId, int userContractId, CancellationToken cancellationToken);
    Task<ContractMarketType?> GetContractMarketTypeById(int id, CancellationToken cancellationToken);
}
