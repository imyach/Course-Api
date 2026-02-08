using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Persistance.EntityFrameworkConfiguration
{
    public class TestResultConfiguration : IEntityTypeConfiguration<TestResult>
    {
        public void Configure(EntityTypeBuilder<TestResult> builder)
        {
            builder.HasKey(prog => prog.Id);
            builder.HasIndex(prog => prog.Id).IsUnique();
            builder.Property(prog => prog.Id).HasMaxLength(250);

            builder.Property(prog => prog.Score).HasPrecision(3).IsRequired();
            builder.Property(prog => prog.IsPassed).IsRequired();
            builder.Property(prog => prog.UserId).IsRequired();
            builder.Property(prog => prog.TestId).IsRequired();
            builder.Property(prog => prog.ProgressMaterialId).IsRequired();

            builder.HasOne(user => user.User)
                .WithMany(progs => progs.TestResult)
                .HasForeignKey(k => k.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(test => test.Test)
                .WithMany(testRes => testRes.TestResult)
                .HasForeignKey(k => k.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(progMat => progMat.ProgressMaterial)
               .WithMany(testRes => testRes.TestResult)
               .HasForeignKey(k => k.ProgressMaterialId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
