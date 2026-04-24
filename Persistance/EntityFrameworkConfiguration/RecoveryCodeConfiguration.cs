using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.EntityFrameworkConfiguration
{
    public class RecoveryCodeConfiguration : IEntityTypeConfiguration<RecoveryCode>
    {
        public void Configure(EntityTypeBuilder<RecoveryCode> builder)
        {
            builder.HasKey(code => code.Id);
            builder.HasIndex(code => code.Id).IsUnique();
            builder.Property(code => code.Id).HasMaxLength(250);
            builder.Property(code => code.UserId).IsRequired();
            builder.Property(code => code.CodeHash).IsRequired();
            builder.Property(code => code.ExpirationTime).IsRequired();
            builder.Property(code => code.IsUsedEarlier).IsRequired();

            builder.HasOne(code => code.User)
               .WithOne(user => user.RecoveryCode)
               .HasForeignKey<RecoveryCode>(key => key.UserId);
        }
    }
}
