using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.EntityFrameworkConfiguration
{
    public class ProgressModuleConfiguration : IEntityTypeConfiguration<ProgressModule>
    {
        public void Configure(EntityTypeBuilder<ProgressModule> builder)
        {
            builder.HasKey(prog => prog.Id);
            builder.HasIndex(prog => prog.Id).IsUnique();
            builder.Property(prog => prog.Id).HasMaxLength(250);
            builder.Property(prop => prop.Status).HasMaxLength(20).IsRequired();
            builder.Property(prog => prog.UserId).IsRequired();
            builder.Property(prog => prog.ModuleId).IsRequired();
            builder.Property(prog => prog.ProgressUserId).IsRequired();
            builder.Property(prog => prog.StartedAt).IsRequired(false);
            builder.Property(prog => prog.Order).IsRequired();

            builder.HasOne(user => user.User)
                .WithMany(progs => progs.ProgressModule)
                .HasForeignKey(k => k.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(prog => prog.Module)
                .WithMany(progs => progs.ProgressModules)
                .HasForeignKey(k => k.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(prog => prog.ProgressUser)
               .WithMany(progs => progs.ProgressModule)
               .HasForeignKey(k => k.ProgressUserId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
