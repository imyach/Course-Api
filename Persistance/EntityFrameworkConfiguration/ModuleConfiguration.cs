using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.EntityFrameworkConfiguration
{
    public class ModuleConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.HasKey(mod => mod.Id);
            builder.HasIndex(mod => mod.Id).IsUnique();
            builder.Property(mod => mod.Id).HasMaxLength(250);
            builder.Property(mod => mod.CourseId).IsRequired();
            builder.Property(mod => mod.Title).IsRequired().HasMaxLength(100);
            builder.Property(mod => mod.Order).IsRequired();

            builder.HasOne(course => course.Course)
                .WithMany(modules => modules.Modules)
                .HasForeignKey(k => k.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
