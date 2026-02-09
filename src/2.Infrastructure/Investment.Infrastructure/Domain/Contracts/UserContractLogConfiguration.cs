using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Contracts.Entities;
using Investment.Domain.Contracts.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Investment.Infrastructure.Domain.Contracts;

public class UserContractLogConfiguration : IEntityTypeConfiguration<UserContractLog>
{
    public void Configure(EntityTypeBuilder<UserContractLog> builder)
    {
        builder.HasKey(UserContractLog => UserContractLog.Id);

        builder.Property(UserContractLog => UserContractLog.Status)
            .HasMaxLength(1000)
            .IsRequired(true);

        builder.Property(UserContractLog => UserContractLog.Description)
            .HasMaxLength(100)
            .IsRequired(true);

        builder.Property(UserContractLog => UserContractLog.CreateDateTime)
            .HasColumnType("timestamp without time zone")
            .IsRequired(true);
    }
}
