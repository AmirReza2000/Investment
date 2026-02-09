using Investment.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Investment.Infrastructure.Domain.Users;

internal class RefreshTokenEntityTypeConfiguration
     : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(refreshToken => refreshToken.Id);

        builder.HasIndex(refreshToken => refreshToken.RefreshTokenHash)
               .IsUnique(true);

        builder.Property(refreshToken => refreshToken.DeviceName)
               .HasMaxLength(50)
               .IsUnicode(false)
               .IsRequired(false);

        builder.Property(refreshToken => refreshToken.IpAddress)
               .HasMaxLength(50)
               .IsUnicode(false)
               .IsRequired(false);

        builder.Property(refreshToken => refreshToken.RefreshTokenHash)
               .HasMaxLength(130)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(refreshToken => refreshToken.UserAgent)
               .HasMaxLength(50)
               .IsUnicode(true)
               .IsRequired(false);

        builder.HasOne(refreshToken => refreshToken.ReplacedByToken)
               .WithOne()
               .HasForeignKey<RefreshToken>(refreshToken => refreshToken.ReplacedByTokenId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(refreshToken => refreshToken.RevokeReason);

        builder.Property(refreshToken => refreshToken.Revoked);

        builder.Property(refreshToken => refreshToken.ExpiresAt)
                   .HasColumnType("timestamp without time zone");

        builder.Property(refreshToken => refreshToken.RevokedAt)
                    .HasColumnType("timestamp without time zone");

        builder.Property(refreshToken => refreshToken.CreatedAt)
            .HasColumnType("timestamp without time zone");

    }
}
