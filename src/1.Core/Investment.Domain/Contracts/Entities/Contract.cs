using Investment.Domain.Common;
using Investment.Domain.Contracts.Enums;
using Investment.Domain.Contracts.Rules;

namespace Investment.Domain.Contracts.Entities;

public class Contract : AggregateRoot<int>
{
    public string Title { get; private set; } = string.Empty;

    public string ImageName { get; private set; } = string.Empty;

    public decimal Rate { get; private set; }

    public short MinDurationOfDay { get; private set; }

    public int MinAmount { get; private set; }

    public int? MaxAmount { get; private set; }

    public bool IsActive { get; private set; }

    public Metadata? Metadata { get; private set; }

    public int UserId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdateAt { get; private set; }

    private IList<UserContract> _userContracts = [];

    public IReadOnlyList<UserContract> UserContracts => _userContracts.AsReadOnly();

    private Contract()
    {

    }

    private Contract(string title,
                    string imageName,
                    decimal rate,
                    short minDurationOfDay,
                    int minAmount,
                    int? maxAmount,
                    int userId,
                    Metadata? metadata)
    {
        Title = title;
        ImageName = imageName;
        Rate = rate;
        MinDurationOfDay = minDurationOfDay;
        MinAmount = minAmount;
        MaxAmount = maxAmount;
        UserId = userId;
        IsActive = false;
        CreatedAt = DateTime.Now;
        Metadata = metadata;
    }

    public static Contract Create(string title,
                    string imageName,
                    decimal rate,
                    short minDurationOfDay,
                    int minAmount,
                    int? maxAmount,
                    int userId,
                    Metadata? metadata)
    {

        if (minAmount < 0 || maxAmount < 0 || minAmount >= maxAmount) CheckRuleStatic(new GeneralBusinessRule("the maximum value must exceed the minimum value.", BusinessRuleCodeEnum.None));

        if (minDurationOfDay < 0) CheckRuleStatic(new GeneralBusinessRule("min Duration Of Day must must great than zero", BusinessRuleCodeEnum.None));

        if (rate <= 0) CheckRuleStatic(new GeneralBusinessRule("rate must must great than zero", BusinessRuleCodeEnum.None));

        var newContract = new Contract(
            title: title,
            imageName: imageName,
            rate: rate,
            minDurationOfDay: minDurationOfDay,
            minAmount: minAmount,
            maxAmount: maxAmount,
            userId: userId,
            metadata: metadata);

        return newContract;
    }

    public UserContract CreateContractForUser(decimal amount,
        int durationOfDay,
        int userId,
        ContractDurationTypeEnum contractDurationType,
        ContractMarketType contractMarketType
        )
    {
        CheckRule(new ContactMustBeActiveRule(this));

        CheckRule(new ValidRangeAmountContractRule(amount, MaxAmount, MinAmount));

        CheckRule(new ValidMinDurationContractRule(MinDurationOfDay, durationOfDay));

        CheckRule(new ContractMarketTypeMustBeActiveRule(contractMarketType));

        var userContract = UserContract.Create(
            amount: amount,
            durationOfDay: durationOfDay,
            userId: userId,
            contractDurationType: contractDurationType,
            contractMarketType: contractMarketType,
            contractRate: Rate);

        _userContracts.Add(userContract);

        return userContract;
    }

    public void ApproveUserContract(int userContractId)
    {
        var userContract = _userContracts.FirstOrDefault(userContract => userContract.Id == userContractId);

        if (userContract is null)
        {
            CheckRule(
                new GeneralBusinessRule($"User Contract with id:{userContractId} not found.",
                BusinessRuleCodeEnum.EntityNotFound));
        }

        userContract!.Approve();
    }

    public void RejectUserContract(int userContractId, string description, out int userId)
    {
        var userContract = _userContracts.FirstOrDefault(userContract => userContract.Id == userContractId);

        if (userContract is null)
        {
            CheckRule(
                new GeneralBusinessRule($"User Contract with id:{userContractId} not found.",
                BusinessRuleCodeEnum.EntityNotFound));
        }

        userContract!.Reject(description);

        userId = userContract.UserId;
    }

    public void Update(string title, decimal rate, int minAmount, int? maxAmount, short minDurationOfDay, string? imageName, bool isActive, Metadata? metadata)
    {
        Title = title;
        Rate = rate;
        MinAmount = minAmount;
        MaxAmount = maxAmount;
        MinDurationOfDay = minDurationOfDay;

        if (imageName is not null) ImageName = imageName;

        UpdateAt = DateTime.Now;
        IsActive = isActive;
        Metadata = metadata;
    }

}

public class Metadata
{
    public string? Label { get; set; }
}