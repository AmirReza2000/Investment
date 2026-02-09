using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Users;
using Investment.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Investment.Infrastructure.Domain.Users;

internal class UserEntityTypeConfiguration
    : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);

        builder.HasIndex(user => user.EmailAddress)
               .IsUnique(true);

        builder.Property(user => user.EmailAddress)
               .HasMaxLength(320)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(user => user.HashedPassword)
               .IsUnicode(false)
               .IsRequired();

        builder.HasMany(user => user.RefreshTokens)
               .WithOne(rt => rt.User)
               .HasForeignKey(rt => rt.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(u => u.RefreshTokens)
               .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(user => user.ValidationTokens)
               .WithOne(rt => rt.User)
               .HasForeignKey(rt => rt.UserId)
               .IsRequired(true)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(u => u.ValidationTokens)
               .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(user => user.CreateDatetime)
               .HasColumnType("timestamp without time zone");
    }
}
