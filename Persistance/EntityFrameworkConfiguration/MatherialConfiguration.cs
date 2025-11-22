using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.EntityFrameworkConfiguration
{
    public class MatherialConfiguration : IEntityTypeConfiguration<Matherial>
    {
        public void Configure(EntityTypeBuilder<Matherial> builder)
        {
            builder.HasKey(math => math.Id);
            builder.HasIndex(math => math.Id).IsUnique();
            builder.Property(math => math.Id).HasMaxLength(250);
            builder.Property(math => math.IdModule).IsRequired();
            builder.Property(math => math.Title).IsRequired().HasMaxLength(50);
            builder.Property(math => math.Order).IsRequired();

            builder.HasOne(mod => mod.Module)
                .WithMany(maths => maths.Matherials)
                .HasForeignKey(k => k.IdModule);
        }
    }
}
