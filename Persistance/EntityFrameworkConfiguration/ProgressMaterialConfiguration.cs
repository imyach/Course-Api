using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.EntityFrameworkConfiguration
{
    public class ProgressMaterialConfiguration : IEntityTypeConfiguration<ProgressMaterial>
    {
        public void Configure(EntityTypeBuilder<ProgressMaterial> builder)
        {
            builder.HasKey(prog => prog.Id);
            builder.HasIndex(prog => prog.Id).IsUnique();
            builder.Property(prog => prog.Id).HasMaxLength(250);
            builder.Property(prop => prop.Status).HasMaxLength(20).IsRequired();
            builder.Property(prog => prog.UserId).IsRequired();
            builder.Property(prog => prog.MaterialId).IsRequired();
            builder.Property(prog => prog.StartedAt).IsRequired(false);
            builder.Property(prog => prog.ProgressModuleId).IsRequired();
            builder.Property(prog => prog.Order).IsRequired();

            builder.HasOne(user => user.User)
                .WithMany(progs => progs.ProgressMaterial)
                .HasForeignKey(k => k.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(matherial => matherial.Material)
                .WithMany(progs => progs.ProgressMaterial)
                .HasForeignKey(k => k.MaterialId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(prog => prog.ProgressModule)
               .WithMany(progs => progs.ProgressMaterial)
               .HasForeignKey(k => k.ProgressModuleId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
