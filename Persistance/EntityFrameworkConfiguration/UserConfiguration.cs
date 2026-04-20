using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.EntityFrameworkConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.Id);
            builder.HasIndex(user => user.Id).IsUnique();
            builder.Property(user => user.Id).HasMaxLength(250);
            builder.Property(user => user.NameUser).HasMaxLength(50).IsRequired();
            builder.Property(user => user.Login).HasMaxLength(30).IsRequired();
            builder.Property(user => user.HashPassword).HasMaxLength(250).IsRequired();
            builder.Property(user => user.Email).HasMaxLength(50).IsRequired();
            builder.Property(user => user.CreatedAt).IsRequired();
            builder.Property(user => user.RoleId).IsRequired();
            builder.Property(user => user.IsActive).IsRequired();
            builder.Property(user => user.PhoneNumber).HasMaxLength(18);

            builder.HasOne(role => role.Role)
                .WithMany(users => users.Users)
                .HasForeignKey(k => k.RoleId);

        }
    }
}
