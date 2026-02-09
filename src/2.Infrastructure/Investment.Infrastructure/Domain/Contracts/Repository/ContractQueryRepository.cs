using System.Runtime.CompilerServices;
using Investment.Application.Contracts.Queries.ContractMarketTypes;
using Investment.Application.Contracts.Queries.Contracts;
using Investment.Application.Contracts.Queries.ExpectedProfit;
using Investment.Application.Contracts.Queries.UserContractsForAdmin;
using Investment.Application.Contracts.Queries.UserContracts;
using Investment.Application.Users.Queries.Dashboard;
using Investment.Application.Utilities.Exceptions;
using Investment.Application.Utilities.PaginatedQuery;
using Investment.Application.Utilities.QueryRepositories;
using Investment.Domain.Contracts.Entities;
using Investment.Domain.Contracts.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Investment.Infrastructure.Domain.Contracts.Repository;

public class ContractQueryRepository : IContractQueryRepository
{
    private IQueryable<Contract> _contracts => _dbContext.Contracts.AsNoTracking();
    private IQueryable<UserContract> _userContracts => _dbContext.UserContracts.AsNoTracking();
    private IQueryable<ContractMarketType> _contractMarketType => _dbContext.ContractMarketTypes.AsNoTracking();

    private readonly InvestmentDbContext _dbContext;

    public ContractQueryRepository(InvestmentDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public Task<ContractsResponse[]> GetContractsPaginated(ContractsQuery request, CancellationToken cancellationToken)
    {
        var query = _contracts;

        if (request.ActiveFilter is not null)
        {
            query = query.Where(contract => contract.IsActive == request.ActiveFilter);
        }

        return query.ApplyPaging(request).Select(contract =>
        new ContractsResponse
        {
            Id = contract.Id,
            Title = contract.Title,
            ImageName = contract.ImageName,
            Rate = contract.Rate,
            MinDurationOfDay = contract.MinDurationOfDay,
            MinAmount = contract.MinAmount,
            MaxAmount = contract.MaxAmount,
            IsActive = contract.IsActive,
            Metadata = contract.Metadata
        }).ToArrayAsync(cancellationToken: cancellationToken);
    }

    public Task<ContractMarketType?> GetContractMarketTypeAsync(int contractMarketTypeId, CancellationToken cancellationToken)
    {
        return _contractMarketType
                 .FirstOrDefaultAsync(contractMarketType =>
                 contractMarketType.Id == contractMarketTypeId,
                 cancellationToken: cancellationToken);
    }

    public Task<ContractMarketTypeResponse[]> GetContractMarketTypesAsync(ContractMarketTypesQuery request, CancellationToken cancellationToken)
    {
        var query = _contractMarketType;

        if (request.ActiveFilter is not null)
        {
            query = query.Where(contractMarketType => contractMarketType.IsActive == request.ActiveFilter);
        }

        return query.Select(contractMarketType =>
        new ContractMarketTypeResponse(
            contractMarketType.Id,
            contractMarketType.Title,
            contractMarketType.IsActive,
            contractMarketType.Rate)).ToArrayAsync(cancellationToken: cancellationToken);
    }

    public Task<UserContractResponse[]> GetUserContractsForUserAsync(int userId, UserContractsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<UserContract> query = _userContracts
            .Include(userContract => userContract.Contract)
            .Include(userContract => userContract.ContractMarketType)
            .Include(userContract => userContract.UserContractProfits)
            .Where(userContract => userContract.UserId == userId);

        if (request.TitleContractFilter is not null)
        {
            query = query.Where(userContract =>
                userContract.Contract!
                .Title.Contains(request.TitleContractFilter.Trim()));
        }

        if (request.TitleMarketTypeFilter is not null)
        {
            query = query.Where(userContract =>
                userContract.ContractMarketType!
                .Title.Contains(request.TitleMarketTypeFilter.Trim()));
        }

        if (request.StatusFilter is not null)
        {
            query = query.Where(userContract =>
                userContract.Status == request.StatusFilter);
        }

        if (request.AcceptAtDateTimeFilter is not null)
        {
            query = query.Where(userContract =>
               userContract.AcceptedAt >= request.AcceptAtDateTimeFilter);
        }
        return UserContractResponseExpression(query.ApplyPaging(request)).ToArrayAsync(cancellationToken);
    }

    public Task<UserContractResponse?> GetUserContractForUserAsync(int userId, int userContractId, CancellationToken cancellationToken)
    {
        IQueryable<UserContract> query = _userContracts
           .Include(userContract => userContract.Contract)
           .Include(userContract => userContract.ContractMarketType)
           .Include(userContract => userContract.UserContractProfits)
           .Where(userContract => userContract.UserId == userId && userContract.Id == userContractId);

        return UserContractResponseExpression(query).FirstOrDefaultAsync(cancellationToken);

    }
    public static IQueryable<UserContractResponse> UserContractResponseExpression(IQueryable<UserContract> query)
    {
        return query.Select(userContract => new UserContractResponse
        {
            Id = userContract.Id,
            ContractId = userContract.ContractId,
            ContractTitle = userContract.Contract!.Title,
            ContractImage = userContract.Contract.ImageName,
            DurationOfMonth = userContract.DurationOfDay / 30,
            CreatedAt = userContract.CreatedAt,
            Amount = userContract.Amount,
            Rate = userContract.CalculatedRate,

            SumRateProfitsReceived = userContract.UserContractProfits
                            .Where(userContractProfit => userContractProfit.UserContractId == userContract.Id)
                            .Where(userContractProfit => userContractProfit.DepositedAt != default)
                            .Select(userContractProfit => userContractProfit.Rate)
                            .Sum(),

            NextMonthsProfitRate = userContract.UserContractProfits
                        .Where(userContractProfit => userContractProfit.UserContractId == userContract.Id)
                        .Where(userContractProfit => userContractProfit.DepositedAt == default)
                        .OrderBy(userContractProfit => userContractProfit.EffectiveDate)
                        .Select(userContractProfit => userContractProfit.Rate)
                        .FirstOrDefault(),

            ContractDurationType = userContract.ContractDurationType,
            Status = userContract.Status,
            UpdatedAt = userContract.UpdatedAt,
            AcceptedAt = userContract.AcceptedAt,
            MarketTypeTitle = userContract.ContractMarketType!.Title
        });
    }

    public async Task<ExpectedProfitResponse> GetExpectedProfitAsync(int contractId, int amount, int durationOfDay, int marketTypeId, ContractDurationTypeEnum contractDurationType, CancellationToken cancellationToken)
    {

        decimal rateOfContract = await _contracts
            .Where(contract => contract.Id == contractId)
            .Select(contract => contract.Rate)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (rateOfContract == default) throw new EntityNotFoundException<Contract>();

        decimal rateOfMarketType = await _contractMarketType
            .Where(contractMarketType => contractMarketType.Id == marketTypeId)
            .Select(contractMarketType => contractMarketType.Rate)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (rateOfMarketType == default) throw new EntityNotFoundException<ContractMarketType>();

        int durationOfMonth = durationOfDay / 30;

        return new ExpectedProfitResponse
        {
            Amount = amount,
            DurationOfMonth = durationOfMonth,
            ProfitRate = UserContract.CalculateSumRate(
                durationOfMonth,
                contractDurationType,
                rateOfMarketType,
                rateOfContract),
        };
    }

    public Task<UserContractsForAdminResponse[]> GetUserContractsForAdminAsync(int? userContractId, int? userId, CancellationToken cancellationToken)
    {
        var query = _userContracts;

        if (userContractId is not null)
        {
            query = query.Where(userContract => userContract.Id == userContractId);
        }

        if (userId is not null)
        {
            query = query.Where(userContract => userContract.UserId == userId);
        }

        return query.Select(userContract =>
            new UserContractsForAdminResponse
            {
                UserContractId = userContract.Id,
                ContractId = userContract.ContractId,
                DurationOfDay = userContract.DurationOfDay,
                Status = userContract.Status,
                AcceptedAt = userContract.AcceptedAt,
                ContractDurationType = userContract.ContractDurationType,
                Amount = userContract.Amount,
                CalculatedRate = userContract.CalculatedRate,
                CreatedAt = userContract.CreatedAt,
                UpdatedAt = userContract.UpdatedAt,
                UserContractLogs = userContract.UserContractLogs.Select(userContractLog => new UserContractLogForAdminResponse
                {
                    UserContractLogId = userContractLog.Id,
                    Status = userContractLog.Status,
                    CreateDateTime = userContractLog.CreateDateTime,
                    Description = userContractLog.Description
                }).ToArray(),
                UserContractProfits = userContract.UserContractProfits.Select(userContractProfit =>
                new UserContractProfitForAdminResponse
                {
                    UserContractProfitId = userContractProfit.Id,
                    DepositedAt = userContractProfit.DepositedAt,
                    EffectiveDate = userContractProfit.EffectiveDate,
                    Rate = userContractProfit.Rate,
                }).ToArray(),
                UserId = userContract.UserId,
            }).ToArrayAsync(cancellationToken: cancellationToken);
    }

    public Task<UserContract[]> GetUserContractsWithProfitsAsync(int userId, CancellationToken cancellationToken)
    {
        return _userContracts
              .Include(userContract => userContract.UserContractProfits)
              .Where(userContract => userContract.UserId == userId).ToArrayAsync(cancellationToken);
    }

    public Task<ContractResponse?> GetContractByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _contracts
            .Where(contract => contract.Id == id)
            .Select(contract => new ContractResponse
            {
                Id = contract.Id,
                Title = contract.Title,
                ImageName = contract.ImageName,
                Rate = contract.Rate,
                MinDurationOfDay = contract.MinDurationOfDay,
                MinAmount = contract.MinAmount,
                MaxAmount = contract.MaxAmount
            }).FirstOrDefaultAsync(cancellationToken);
    }
}
