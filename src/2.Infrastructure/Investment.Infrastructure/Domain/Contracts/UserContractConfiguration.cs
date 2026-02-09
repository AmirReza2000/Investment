using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Contracts.Entities;
using Investment.Domain.Contracts.Enums;
using Investment.Domain.Users.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Investment.Infrastructure.Domain.Contracts;

public class UserContractConfiguration : IEntityTypeConfiguration<UserContract>
{
    public void Configure(EntityTypeBuilder<UserContract> builder)
    {
        builder.HasKey(userContract => userContract.Id);

        builder.Property(userContract => userContract.Amount)
            .HasPrecision(28, 2);

        builder.Property(userContract => userContract.CalculatedRate)
         .HasPrecision(28, 2);

        builder.Property(userContract => userContract.ContractDurationType)
            .HasMaxLength(1000)
            .IsRequired(true);

        builder.Property(userContract => userContract.Status)
            .HasMaxLength(1000)
            .IsRequired(true);

        builder.Property(userContract => userContract.DurationOfDay)
            .HasMaxLength(4000)
            .IsRequired(true);

        builder.Property(userContract => userContract.UserId)
                .IsRequired(true);

        builder.Property(userContract => userContract.CreatedAt)
            .HasColumnType("timestamp without time zone")
            .IsRequired(true);

        builder.Property(userContract => userContract.UpdatedAt)
            .HasColumnType("timestamp without time zone");

        builder.Property(userContract => userContract.AcceptedAt)
            .HasColumnType("timestamp without time zone");

        builder.HasOne(userContract => userContract.ContractMarketType)
            .WithMany()
            .HasForeignKey(userContract => userContract.ContractMarketTypeId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(userContract => userContract.UserContractLogs)
            .WithOne(userContractLog => userContractLog.UserContract)
            .HasForeignKey(userContractLog => userContractLog.UserContractId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(userContract => userContract.UserContractLogs)
              .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(userContract => userContract.UserContractProfits)
            .WithOne()
            .HasForeignKey(userContractProfit => userContractProfit.UserContractId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(userContract => userContract.UserContractProfits)
              .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
