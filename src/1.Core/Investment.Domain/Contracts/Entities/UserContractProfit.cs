using Investment.Domain.Common;

namespace Investment.Domain.Contracts.Entities;

public class UserContractProfit : Entity<int>
{
    public decimal Rate { get; private set; }

    public DateTime? DepositedAt { get; private set; }

    public DateTime EffectiveDate { get; private set; }

    public int UserContractId { get; private set; }

    internal UserContractProfit(decimal rate, DateTime effectiveDate, int userContractId)
    {
        Rate = rate;
        EffectiveDate = effectiveDate;
        UserContractId = userContractId;
        DepositedAt = default;
    }

    private UserContractProfit()
    {
    }

    internal void SetDepositDate()
    {
        DepositedAt = DateTime.Now;
    }
}