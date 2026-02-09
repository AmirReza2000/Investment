using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Investment.Infrastructure.Domain.Contracts;

public class ContractConfiguration : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(contract => contract.MinAmount)
            .IsRequired();

        builder.Property(contract => contract.MaxAmount)
            .IsRequired(false);

        builder.Property(contract => contract.UserId)
            .IsRequired();

        builder.Property(contract => contract.MinDurationOfDay)
            .IsRequired();

        builder.Property(contract => contract.ImageName)
            .IsRequired();

        builder.Property(contract => contract.Rate)
            .HasPrecision(28, 2)
            .IsRequired();

        builder.Property(contract => contract.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(contract => contract.IsActive)
            .IsRequired();

        builder.Property(contract => contract.Metadata)
            .HasColumnType("jsonb")
            .IsRequired(false);

        builder.Property(contract => contract.CreatedAt)
            .HasColumnType("timestamp without time zone")
            .IsRequired(true);

        builder.Property(contract => contract.UpdateAt)
            .HasColumnType("timestamp without time zone")
            .IsRequired(false);

        builder.HasMany(contract => contract.UserContracts)
            .WithOne(userContract => userContract.Contract)
            .HasForeignKey(userContract => userContract.ContractId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(contract => contract.UserContracts)
               .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
