using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities.Exceptions;
using Investment.Domain.Contracts.Entities;
using Investment.Domain.Contracts.Repository;
using Investment.Infrastructure.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Investment.Infrastructure.Domain.Contracts.Repository;

public class ContractCommandRepository :
    CommandRepository<Contract, int>,
    IContractCommandRepository
{
    private readonly InvestmentDbContext _dbContext;

    public ContractCommandRepository(InvestmentDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddMarketTypeContract(ContractMarketType contractMarketType, CancellationToken cancellationToken)
    {
        await _dbContext.ContractMarketTypes.AddAsync(contractMarketType, cancellationToken);
    }

    public async Task AssertContractMarketTypeNotExistAsync(string title, CancellationToken cancellationToken)
    {
        var isContractMarketTypeExist = await _dbContext.ContractMarketTypes.Where(contractMarketType => contractMarketType.Title == title).AnyAsync();

        if (isContractMarketTypeExist) throw new EntityDuplicateException<ContractMarketType>(nameof(ContractMarketType.Title));

    }

    public async Task AssertContractTitleNotExistAsync(string title, CancellationToken cancellationToken)
    {
        var isContractExist = await _dbContext.Contracts.Where(contract => contract.Title == title).AnyAsync();

        if (isContractExist) throw new EntityDuplicateException<Contract>(nameof(Contract.Title));
    }

    public async Task AssertContractImageNameNotExistAsync(string imageName, CancellationToken cancellationToken)
    {
        var isContractExist = await _dbContext.Contracts.Where(contract => contract.ImageName == imageName).AnyAsync();

        if (isContractExist) throw new EntityDuplicateException<Contract>(nameof(Contract.ImageName));
    }

    public Task<Contract?> GetByIdWithSpecificUserContractAsync(int contractId, int userContractId, CancellationToken cancellationToken)
    {
        return _dbContext.Contracts
            .Include(contract => contract.UserContracts.Where(userContract => userContract.Id == userContractId))
            .Where(contract => contract.Id == contractId).FirstOrDefaultAsync(cancellationToken);
    }

    public Task<ContractMarketType?> GetContractMarketTypeById(int id, CancellationToken cancellationToken)
    {
        return _dbContext.ContractMarketTypes.FirstOrDefaultAsync(contract => contract.Id == id, cancellationToken);
    }

}
