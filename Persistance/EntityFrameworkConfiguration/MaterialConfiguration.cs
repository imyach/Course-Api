using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.EntityFrameworkConfiguration
{
    public class MaterialConfiguration : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.HasKey(math => math.Id);
            builder.HasIndex(math => math.Id).IsUnique();
            builder.Property(math => math.Id).HasMaxLength(250);
            builder.Property(math => math.ModuleId).IsRequired();
            builder.Property(math => math.Title).IsRequired().HasMaxLength(100);
            builder.Property(math => math.Order).IsRequired();

            builder.HasOne(mod => mod.Module)
                .WithMany(maths => maths.Materials)
                .HasForeignKey(k => k.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
