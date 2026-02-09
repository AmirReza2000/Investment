using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Investment.Infrastructure.Domain.Contracts;

public class ContractMarketTypeConfiguration : IEntityTypeConfiguration<ContractMarketType>
{
    public void Configure(EntityTypeBuilder<ContractMarketType> builder)
    {
        builder
            .HasKey(contractMarketType => contractMarketType.Id);

        builder.Property(contractMarketType => contractMarketType.IsActive)
            .IsRequired(true);

        builder.Property(contractMarketType => contractMarketType.Rate)
            .HasPrecision(28, 2)
            .IsRequired(true);

        builder.Property(contractMarketType => contractMarketType.Title)
            .HasMaxLength(100)
            .IsRequired(true);

        builder.Property(UserContractLog => UserContractLog.CreatedAt)
            .HasColumnType("timestamp without time zone")
            .IsRequired(true);

        builder.Property(UserContractLog => UserContractLog.UpdatedAt)
            .HasColumnType("timestamp without time zone");
    }
}
