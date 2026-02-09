namespace Investment.Application.Contracts.Queries.ExpectedProfit;

public class ExpectedProfitResponse
{
    public required decimal Amount { get; set; }

    public required int  DurationOfMonth{ get; set; }

    public required decimal ProfitRate { get; set; }

    public  decimal MonthlyProfit => ProfitRate * Amount/100;

    public decimal TotalEstimatedProfit  => MonthlyProfit * DurationOfMonth ;

    public decimal TotalPayoutAtMaturity => TotalEstimatedProfit + Amount;
}