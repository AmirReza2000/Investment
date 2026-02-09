using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Contracts.Enums;

namespace Investment.Domain.Contracts;

public static class ContractDurationType
{
    public static Dictionary<Guid, RateDurationType> RateDurationTypeValues { get; }

    static ContractDurationType()
    {
        RateDurationTypeValues = new Dictionary<Guid, RateDurationType>
        {
            {new Guid("09A266C1-1A15-4F61-9416-D83DD3A42C9F"), new RateDurationType(ContractDurationTypeEnum.EndOfContract, 1, 1m) }
        };    
    }
}

public record  RateDurationType( 
    ContractDurationTypeEnum Item1, 
    int DurationOfDay,
    decimal Rate);