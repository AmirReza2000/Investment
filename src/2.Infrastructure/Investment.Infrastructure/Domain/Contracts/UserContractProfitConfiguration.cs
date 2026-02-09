using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Investment.Infrastructure.Domain.Contracts;

public class UserContractProfitConfiguration : IEntityTypeConfiguration<UserContractProfit>
{
    public void Configure(EntityTypeBuilder<UserContractProfit> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(entity => entity.Rate)
            .HasPrecision(28, 2)
            .IsRequired();

        builder.Property(entity => entity.DepositedAt)
            .HasColumnType("timestamp without time zone")
            .IsRequired(false);

        builder.Property(entity => entity.EffectiveDate)
            .HasColumnType("timestamp without time zone")
            .IsRequired();
    }
}
