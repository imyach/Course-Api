using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.EntityFrameworkConfiguration
{
    public class ProgressUserConfiguration : IEntityTypeConfiguration<ProgressUser>
    {
        public void Configure(EntityTypeBuilder<ProgressUser> builder)
        {
            builder.HasKey(prog => prog.Id);
            builder.HasIndex(prog => prog.Id).IsUnique();
            builder.Property(prog => prog.Id).HasMaxLength(250);
            builder.Property(prop => prop.Status).HasMaxLength(20).IsRequired();
            builder.Property(prog => prog.StartedAt).IsRequired();
            builder.Property(prog => prog.CourseId).IsRequired();
            builder.Property(prog => prog.UserId).IsRequired();

            builder.HasOne(user => user.User)
                .WithMany(progs => progs.ProgressUsers)
                .HasForeignKey(k => k.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(course => course.Course)
                .WithMany(progs => progs.ProgressUsers)
                .HasForeignKey(k => k.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
