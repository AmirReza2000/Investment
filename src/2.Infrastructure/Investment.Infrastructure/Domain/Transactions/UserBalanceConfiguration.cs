using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Transactions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Investment.Infrastructure.Domain.Transactions;

public class UserBalanceConfiguration : IEntityTypeConfiguration<UserBalance>
{
    public void Configure(EntityTypeBuilder<UserBalance> builder)
    {
        builder.HasIndex(entity => entity.UserId)
               .IsUnique();

        builder.Property(entity => entity.Amount)
               .HasPrecision(28, 2);

        builder.Property(entity => entity.UserId);

        builder.Property(entity => entity.Version)
               .IsRowVersion();

        builder.HasMany(entity => entity.Transactions)
               .WithOne(entity => entity.UserBalance)
               .HasForeignKey(entity => entity.UserBalanceId)
               .IsRequired(true)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(entity => entity.Transactions)
               .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
