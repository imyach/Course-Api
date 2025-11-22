using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.EntityFrameworkConfiguration
{
    public class AnswersUserConfiguration : IEntityTypeConfiguration<AnswersUser>
    {
        public void Configure(EntityTypeBuilder<AnswersUser> builder)
        {
            builder.HasKey(ansusr => ansusr.Id);
            builder.HasIndex(ansusr => ansusr.Id).IsUnique();
            builder.Property(ansusr => ansusr.Id).HasMaxLength(250);
            builder.Property(ansusr => ansusr.IdAnswer).IsRequired();
            builder.Property(ansusr => ansusr.IdQuestion).IsRequired();
            builder.Property(ansusr => ansusr.IdUser).IsRequired();

            builder.HasOne(user => user.User)
                .WithMany(ansusrs => ansusrs.AnswersUsers)
                .HasForeignKey(k => k.IdUser);

            builder.HasOne(asn => asn.Answer)
                .WithMany(tests => tests.AnswersUsers)
                .HasForeignKey(k => k.IdAnswer);

            builder.HasOne(ques => ques.Question)
                .WithMany(tests => tests.AnswersUsers)
                .HasForeignKey(k => k.IdQuestion);

        }
    }
}
