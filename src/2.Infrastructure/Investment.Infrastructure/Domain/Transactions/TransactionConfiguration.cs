using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Transactions.Entities;
using Investment.Domain.Transactions.Enums.Transaction;
using Investment.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Investment.Infrastructure.Domain.Transactions
{
    internal class TransactionConfiguration
    : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(entity => entity.TransactionType)
                   .HasMaxLength(Enum.GetValues(typeof(TransactionTypeEnum)).Length);

            builder.Property(entity => entity.TransactionStatus)
                   .HasMaxLength(200);

            builder.Property(entity => entity.ReasonType)
                   .HasMaxLength(Enum.GetValues(typeof(ReasonTypeEnum)).Length);

            builder.Property(entity => entity.Reason)
                   .IsUnicode(false)
                   .HasMaxLength(150);

            builder.Property(entity => entity.Amount)
                   .HasPrecision(28, 2);

            builder.Property(entity => entity.CreateDatetime)
                   .HasColumnType("timestamp without time zone");

            builder.Property(entity => entity.Address)
                   .HasMaxLength(250)
                   .IsRequired(true)
                   .IsUnicode(false);
        }
    }
}
