using Investment.Application.Utilities.Abstractions;
using Investment.Domain.Contracts.Entities;
using Investment.Domain.Contracts.Enums;
using Investment.Domain.Transactions.Entities;
using Investment.Domain.Transactions.Repositories;
using Investment.Infrastructure.Domain.Transactions.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Investment.Infrastructure.Services.Contract;

public class ContractProfitAllocator(
    InvestmentDbContext InvestmentDbContext, 
    IUserBalanceCommandRepository userBalanceCommandRepository, 
    IUnitOfWork unitOfWork,
    ILogger<ContractProfitAllocator> logger)
{

    public async Task AllocateAsync(CancellationToken cancellationToken)
    {
        var today = DateTime.Now.Date;

        const int batchSize = 2;
        int lastId = 0;

        while (true)
        {


            var userContracts = await InvestmentDbContext.UserContracts
                .Where(userContract =>
                                userContract.Id > lastId &&
                                userContract.Status == UserContractStatusEnum.Approved)
                .Where(userContract =>
                                userContract.UserContractProfits.Any(userContractProfit =>
                                userContractProfit.EffectiveDate >= today &&
                                userContractProfit.EffectiveDate < today.AddDays(1) &&
                                userContractProfit.DepositedAt == default))
                .Include(userContract => userContract.UserContractProfits.Where(userContractProfit =>
                                userContractProfit.EffectiveDate >= today &&
                                userContractProfit.EffectiveDate < today.AddDays(1) &&
                                userContractProfit.DepositedAt == default))
                .OrderBy(c => c.Id)
                .Take(batchSize)
                .ToListAsync(cancellationToken);

            if (userContracts.Count == 0)
                break;

            lastId = userContracts[^1].Id;

            foreach (var userContract in userContracts)
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    (int userContractProfitId, decimal profit) = userContract.AllocateProfit();

                    var userBalance = await userBalanceCommandRepository.GetUserBalanceByUserIdAsync(userContract.UserId, cancellationToken)
                      ?? throw new InvalidOperationException($"userBalance  for User({userContract.UserId}) not Found ");

                    userBalance.CreateDepositForContractProfit(userContract.Id, userContractProfitId, profit);

                    await unitOfWork.SaveChangesAsync(cancellationToken);

                    unitOfWork.CommitTransaction();
                }
                catch (Exception ex)
                {
                    unitOfWork.RollbackTransaction();
                    logger.LogError(ex, "The following error occurred while allocating profit to a UserContract with ID {UserContractId}.{ErrorMessage}", userContract.Id, ex.Message);
                }
            }
        }
    }
}
