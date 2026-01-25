using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.EntityFrameworkConfiguration
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(course => course.Id);
            builder.HasIndex(course => course.Id).IsUnique();
            builder.Property(course => course.Id).HasMaxLength(250);
            builder.Property(course => course.Title).IsRequired().HasMaxLength(50);
            builder.Property(course => course.CreatedAt).IsRequired();
            builder.Property(course => course.UserId).IsRequired();
            builder.Property(course => course.Status).IsRequired();

            builder.HasOne(user => user.User)
                .WithMany(courses => courses.Courses)
                .HasForeignKey(k => k.UserId);

        }
    }
}
