using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Contracts.Enums;
using NBitcoin.RPC;

namespace Investment.Application.Contracts.Queries.UserContracts;

public class UserContractResponse
{
    public required int Id { get; set; }

    public required int ContractId { get; set; }

    public required string ContractTitle { get; set; }

    public required string ContractImage { get; set; }

    public required int DurationOfMonth { get; set; }

    public required decimal Amount { get; set; }

    public required decimal Rate { get; set; }

    public decimal MonthlyProfit => Rate * Amount / 100;

    public decimal TotalProfit => Rate * Amount / 100 * DurationOfMonth;

    public decimal TotalAmount => TotalProfit + Amount;

    public required decimal SumRateProfitsReceived { get; set; }

    public float ProfitReceived => (float)(SumRateProfitsReceived * Amount / 100);

    public required decimal NextMonthsProfitRate { get; set; }

    public float NextMonthsProfit => (float)(NextMonthsProfitRate * Amount / 100);

    public double DurationOfDay => Status is not UserContractStatusEnum.Approved ? 0 : (EndOfContract - AcceptedAt).TotalDays;

    public double Elapsed => Status is not UserContractStatusEnum.Approved || IsExpire ? DurationOfDay : (DateTime.Now - AcceptedAt).TotalDays;

    public int PayoutDay => Status is not UserContractStatusEnum.Approved ? 0 : AcceptedAt.Day;

    public float LeftDays => Status is not UserContractStatusEnum.Approved || IsExpire ? 0 : ((float)(EndOfContract - DateTime.Now).TotalDays);

    public double ProgressPercent => Status is not UserContractStatusEnum.Approved ? 0 : Elapsed / DurationOfDay * 100;

    public required ContractDurationTypeEnum ContractDurationType { get; set; }

    public required UserContractStatusEnum Status { get; set; }

    public bool IsExpire => EndOfContract < DateTime.Now;

    public required DateTime CreatedAt { get; set; }

    public DateTime EndOfContract => Status is not UserContractStatusEnum.Approved ? default : AcceptedAt.AddMonths(DurationOfMonth);

    public required DateTime UpdatedAt { get; set; }

    public required DateTime AcceptedAt { get; set; }

    public required string MarketTypeTitle { get; set; }

}
