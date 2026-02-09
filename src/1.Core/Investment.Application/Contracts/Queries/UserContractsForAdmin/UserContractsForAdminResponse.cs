using Investment.Domain.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.Contracts.Queries.UserContractsForAdmin;

public class UserContractsForAdminResponse
{
    public required int UserContractId { get; set; }

    public required int ContractId { get; set; }

    public required decimal CalculatedRate { get; set; }

    public required decimal Amount { get; set; }

    public required int DurationOfDay { get; set; }

    public required ContractDurationTypeEnum ContractDurationType { get; set; }

    public required UserContractStatusEnum Status { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required DateTime UpdatedAt { get; set; }

    public required DateTime AcceptedAt { get; set; }

    public required int UserId { get; set; }

    public required UserContractLogForAdminResponse[] UserContractLogs { get; set; }

    public required UserContractProfitForAdminResponse[] UserContractProfits { get; set; }

}

public class UserContractLogForAdminResponse
{
    public required int UserContractLogId { get; set; }

    public required string Description { get; set; } = string.Empty;

    public required UserContractLogStatusEnum Status { get; set; }

    public required DateTime CreateDateTime { get; set; }
}

public class UserContractProfitForAdminResponse
{
    public required int UserContractProfitId { get; set; }

    public required decimal Rate { get; set; }

    public required DateTime? DepositedAt { get; set; }

    public required DateTime EffectiveDate { get; set; }
}