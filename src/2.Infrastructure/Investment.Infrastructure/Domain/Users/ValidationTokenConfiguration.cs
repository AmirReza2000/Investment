using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Users.Entities;
using Investment.Domain.Users.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Investment.Infrastructure.Domain.Users;

internal class ValidationTokenConfiguration :
    IEntityTypeConfiguration<ValidationToken>
{
    public void Configure(EntityTypeBuilder<ValidationToken> builder)
    {
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.ValidationTokenType)
               .HasMaxLength(Enum.GetNames(typeof(ValidationTokenTypeEnum)).Length)
               .IsRequired(true);

        builder.HasIndex(entity => entity.HashedToken)
               .IsUnique(true);

        builder.Property(entity => entity.Revoked);

        builder.Property(entity => entity.HashedToken)
               .HasMaxLength(130)
               .IsUnicode(false)
               .IsRequired(true);

        builder.Property(entity => entity.UserId);

        builder.Property(entity => entity.ExpireAt)
               .HasColumnType("timestamp without time zone")
               .IsRequired(true);

    }
}
