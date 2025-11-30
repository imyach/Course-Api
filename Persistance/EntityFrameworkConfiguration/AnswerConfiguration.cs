using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.EntityFrameworkConfiguration
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.HasKey(ans => ans.Id);
            builder.HasIndex(ans => ans.Id).IsUnique();
            builder.Property(ans => ans.Id).HasMaxLength(250);
            builder.Property(ans => ans.QuestionId).IsRequired();
            builder.Property(ans => ans.Text).IsRequired();
            builder.Property(ans => ans.IsCorrect).IsRequired();


            builder.HasOne(quest => quest.Question)
                .WithMany(ans => ans.Answers)
                .HasForeignKey(k => k.QuestionId);
        }
    }
}
