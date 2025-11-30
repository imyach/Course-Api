using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.EntityFrameworkConfiguration
{
    public class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.HasKey(test => test.Id);
            builder.HasIndex(test => test.Id).IsUnique();
            builder.Property(test => test.Id).HasMaxLength(250);
            builder.Property(test => test.Title).IsRequired().HasMaxLength(250);

            builder.HasOne(math => math.Matherial)
                .WithMany(tests => tests.Tests)
                .HasForeignKey(k => k.MatherialId);

            builder.HasOne(course => course.Course)
                .WithMany(tests => tests.Tests)
                .HasForeignKey(k => k.CousreId);
        }
    }
}
