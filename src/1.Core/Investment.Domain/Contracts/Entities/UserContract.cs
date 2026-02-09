using Investment.Domain.Common;
using Investment.Domain.Contracts.Enums;
using Investment.Domain.Contracts.Rules;
using System.Security.Cryptography;

namespace Investment.Domain.Contracts.Entities;

public class UserContract : Entity<int>
{
    public decimal CalculatedRate { get; private set; }

    public decimal Amount { get; private set; }

    public int DurationOfDay { get; private set; }

    public ContractDurationTypeEnum ContractDurationType { get; private set; }

    public UserContractStatusEnum Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public DateTime AcceptedAt { get; private set; }

    public int UserId { get; private set; }

    public int ContractMarketTypeId { get; private set; }
    public ContractMarketType? ContractMarketType { get; }

    public int ContractId { get; private set; }
    public Contract? Contract { get; }

    private IList<UserContractLog> _userContractLogs = [];
    public IReadOnlyList<UserContractLog> UserContractLogs => _userContractLogs.AsReadOnly();

    private IList<UserContractProfit> _userContractProfits = [];
    public IReadOnlyList<UserContractProfit> UserContractProfits => _userContractProfits.AsReadOnly();

    private UserContract()
    {

    }

    private UserContract(
        decimal amount,
        decimal sumRate,
        int durationOfDay,
        int userId,
        ContractDurationTypeEnum contractDurationType,
        ContractMarketType contractMarketType)
    {
        Amount = amount;
        CalculatedRate = sumRate;
        DurationOfDay = durationOfDay;
        ContractDurationType = contractDurationType;
        ContractMarketType = contractMarketType;
        ContractMarketTypeId = contractMarketType.Id;
        Status = UserContractStatusEnum.PendingApprove;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
        UserId = userId;
    }

    internal static UserContract Create(decimal amount,
        int durationOfDay,
        int userId,
        ContractDurationTypeEnum contractDurationType,
        ContractMarketType contractMarketType,
        decimal contractRate)
    {
        if (contractDurationType == default || durationOfDay < 30 || contractMarketType.Rate == default)
            CheckRuleStatic(new GeneralBusinessRule("contractDurationTypeEnum or durationOfDay or contractMarketType.Rate out of range", BusinessRuleCodeEnum.None));

        decimal sumRate = CalculateSumRate(durationOfDay / 30, contractDurationType, contractMarketType.Rate, contractRate);

        var userContract = new UserContract(
            amount,
            sumRate,
            durationOfDay,
            userId,
            contractDurationType,
            contractMarketType
            );

        userContract._userContractLogs.Add(new UserContractLog(
            description: "the contract was created for the user.",
            UserContractLogStatusEnum.Create));

        return userContract;
    }

    public static decimal CalculateSumRate(int durationOfMonth, ContractDurationTypeEnum contractDurationType, decimal contractMarketTypeRate, decimal contractRate)
    {

        return GetDurationRate(durationOfMonth) * contractRate * contractMarketTypeRate * GetDurationRate(contractDurationType);
    }

    private static decimal GetDurationRate(ContractDurationTypeEnum contractDurationType)
    {
        return contractDurationType switch
        {
            ContractDurationTypeEnum.EndOfContract => 1.2m,
            _ => 1m,
        };
    }

    private static decimal GetDurationRate(int durationOfMonth)
    {
        return durationOfMonth switch
        {
            >= 1 and <= 2 => 1.0m,
            >= 3 and <= 5 => 1.1m,
            >= 6 and <= 11 => 1.25m,
            >= 12 => 1.0m,
            _ => throw new ArgumentOutOfRangeException(
                nameof(durationOfMonth),
                durationOfMonth,
                "Duration must be between 1 and 12 months.")
        };
    }

    internal void Approve()
    {
        CheckRule(new UserContractMustBePendingApproveRule(this));

        Status = UserContractStatusEnum.Approved;

        _userContractLogs.
            Add(new UserContractLog(
                "contract user approve.", UserContractLogStatusEnum.Approve));


        CreateUserContractProfitLogs();

        AcceptedAt = DateTime.Now;

        UpdatedAt = DateTime.Now;

    }

    internal void Reject(string description)
    {
        CheckRule(new UserContractMustBePendingApproveRule(this));

        _userContractLogs.
            Add(new UserContractLog(
                description, UserContractLogStatusEnum.Reject));

        Status = UserContractStatusEnum.Rejected;

        UpdatedAt = DateTime.Now;

    }

    private void CreateUserContractProfitLogs()
    {
        int durationOfMonth = DurationOfDay / 30;

        if (ContractDurationType == ContractDurationTypeEnum.EndOfContract)
        {
            _userContractProfits.Add(new UserContractProfit(
                CalculatedRate * durationOfMonth,
                DateTime.Now.AddMonths(durationOfMonth),
                Id));
        }
        else
        {
            decimal[] monthlyRates = new decimal[durationOfMonth];
            decimal[] weights = new decimal[durationOfMonth];

            Random random = new Random();

            decimal weightSum = 0;
            for (int i = 0; i < durationOfMonth; i++)
                weightSum += weights[i] = ((decimal)random.NextDouble()) * 0.1m + 0.95m;

            decimal totalCalculatedRate = CalculatedRate * durationOfMonth;

            decimal sumRounded = 0;
            for (int i = 0; i < durationOfMonth; i++)
                sumRounded += monthlyRates[i] =
                    Math.Round((weights[i] / weightSum) * totalCalculatedRate, 2);

            monthlyRates[durationOfMonth - 1] += Math.Round(totalCalculatedRate - sumRounded, 2);

            for (int i = 0; i < monthlyRates.Length; i++)
            {
                _userContractProfits.Add(new UserContractProfit(
               monthlyRates[i],
               DateTime.Now.AddMonths(i + 1),
               Id));
            }
        }

    }

    public (int UserContractProfitId, decimal Profit) AllocateProfit()
    {
        DateTime dateNow = DateTime.Now.Date;

        var userContractProfit = UserContractProfits
            .SingleOrDefault(userContractProfit => userContractProfit.EffectiveDate.Date == dateNow)
            ?? throw new NullReferenceException($"No benefits were found for UserContractId({Id}) on today's date {dateNow}.");

        if (userContractProfit.DepositedAt != default) throw new Exception($"userContractProfitId({userContractProfit.Id}) for UserContractId({Id})  has already been paid on a certain date.");

        userContractProfit.SetDepositDate();

        _userContractLogs.Add(
            new UserContractLog(
                $"deposit profit for userContractProfit({userContractProfit.Id})",
                UserContractLogStatusEnum.ProfitDeposit));

        return (userContractProfit.Id, Amount * userContractProfit.Rate / 100);
    }

    public decimal CalculateTotalProfitExpected()
    {
        if (Status is UserContractStatusEnum.Rejected or UserContractStatusEnum.PendingApprove) return 0;

        return CalculatedRate * Amount * (DurationOfDay / 30) / 100;
    }

    public decimal CalculateLockedAmount()
    {
        if (Status == UserContractStatusEnum.Rejected) return 0;

        return Amount;
    }

    public decimal CalculateUnpaidProfits()
    {
        if (Status is UserContractStatusEnum.Rejected or UserContractStatusEnum.PendingApprove) return 0;

        decimal sumRateFuture = UserContractProfits.Where(profit => profit.DepositedAt == default).Select(profit => profit.Rate).Sum();

        return sumRateFuture * Amount / 100;
    }
}
