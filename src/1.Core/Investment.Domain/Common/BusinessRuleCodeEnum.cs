namespace Investment.Domain.Common;

public enum BusinessRuleCodeEnum
{
    None = 0,

    ValidUsernameAndPassword = 10,

    RefreshTokenValidation = 20,

    RefreshTokenHashValidation = 30,

    UserMustBeActiveForLogin = 40,

    UserMustBeNotVerified = 50,

    UserMustBeVerified = 60,

    ValidVerificationToken = 70,

    ValidRestPasswordTokenShouldNotExist = 80,

    ValidVerifyTokenShouldNotExist = 90,

    WalletAdequacy = 100,

    EntityNotFound = 110,

    WithdrawalApproval = 120,

    WithdrawalMustApproval = 130,

    WithdrawalMustInCreatingStatus = 135,

    TransactionMustBeWithdrawalType = 140,

    TransactionMustBeDepositType = 150,

    ValidRangeAmountContract=160,

    ValidMinDurationContract=170,

    ContractMustBeActive=180,

    ContractMarketTypeMustBeActive=190,

    UserContractMustBePendingApprove=200,

    UserContractMustBeApproved=210,
}