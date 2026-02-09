using Investment.Application.Contracts.Queries.ContractMarketTypes;
using Investment.Application.Contracts.Queries.Contracts;
using Investment.Application.Contracts.Queries.ExpectedProfit;
using Investment.Application.Contracts.Queries.UserContractsForAdmin;
using Investment.Application.Contracts.Queries.UserContracts;
using Investment.Application.Users.Queries.Dashboard;
using Investment.Domain.Contracts.Entities;
using Investment.Domain.Contracts.Enums;

namespace Investment.Application.Utilities.QueryRepositories;

public interface IContractQueryRepository
{
    Task<ContractsResponse[]> GetContractsPaginated(ContractsQuery request, CancellationToken cancellationToken);
    Task<ContractMarketType?> GetContractMarketTypeAsync(int contractMarketTypeId, CancellationToken cancellationToken);
    Task<ContractMarketTypeResponse[]> GetContractMarketTypesAsync(ContractMarketTypesQuery request, CancellationToken cancellationToken);
    Task<UserContractResponse[]> GetUserContractsForUserAsync(int userId, UserContractsQuery request, CancellationToken cancellationToken);
    Task<UserContractResponse?> GetUserContractForUserAsync(int userId, int userContractId, CancellationToken cancellationToken);
    Task<ExpectedProfitResponse> GetExpectedProfitAsync(int contractId, int amount, int durationOfDay, int marketTypeId, ContractDurationTypeEnum contractDurationType, CancellationToken cancellationToken);
    Task<UserContractsForAdminResponse[]> GetUserContractsForAdminAsync(int? userContractId, int? userId, CancellationToken cancellationToken);
    Task<UserContract[]> GetUserContractsWithProfitsAsync(int userId, CancellationToken cancellationToken);
    Task<ContractResponse?> GetContractByIdAsync(int Id, CancellationToken cancellationToken);
}
