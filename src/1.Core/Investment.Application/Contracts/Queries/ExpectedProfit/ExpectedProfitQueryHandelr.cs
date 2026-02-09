
using Investment.Application.Utilities.QueryRepositories;
using MediatR;

namespace Investment.Application.Contracts.Queries.ExpectedProfit;

public class ExpectedProfitQueryHandler(IContractQueryRepository contractQueryRepository)
    : IRequestHandler<ExpectedProfitQuery, ExpectedProfitResponse>
{
    public async Task<ExpectedProfitResponse> Handle(ExpectedProfitQuery request, CancellationToken cancellationToken)
    {
        return await contractQueryRepository.GetExpectedProfitAsync(
            request.ContractId,
            request.Amount,
            request.DurationOfDay,
            request.MarketTypeId,
            request.ContractDurationType,
            cancellationToken);
    }
}
