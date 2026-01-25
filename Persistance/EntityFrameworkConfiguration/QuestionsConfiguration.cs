using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.EntityFrameworkConfiguration
{
    public class QuestionsConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(ques => ques.Id);
            builder.HasIndex(ques => ques.Id).IsUnique();
            builder.Property(ques => ques.Id).HasMaxLength(250);
            builder.Property(ques => ques.Text).IsRequired();
            builder.Property(ques => ques.TestId).IsRequired();

            builder.HasOne(test => test.Test)
                .WithMany(questions => questions.Questions)
                .HasForeignKey(k => k.TestId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
