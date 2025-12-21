using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.EntityFrameworkConfiguration
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(token => token.Id);
            builder.HasIndex(token => token.Id).IsUnique();
            builder.Property(token => token.Id).HasMaxLength(250);
            builder.Property(token => token.CreatedAt).IsRequired();
            builder.Property(token => token.ExpiresIn).IsRequired();
            builder.Property(token => token.UserId).IsRequired();
            builder.Property(token => token.ResreshToken).IsRequired();

            builder.HasOne(token => token.User)
                .WithOne(user => user.RefreshToken)
                .HasForeignKey<RefreshToken>(key => key.UserId);

        }
    }
}
