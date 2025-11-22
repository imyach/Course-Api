using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.EntityFrameworkConfiguration
{
    public class ReviewsConfiguration : IEntityTypeConfiguration<Reviews>
    {
        public void Configure(EntityTypeBuilder<Reviews> builder)
        {
            builder.HasKey(review => review.Id);
            builder.HasIndex(review => review.Id).IsUnique();
            builder.Property(review => review.Id).HasMaxLength(250);
            builder.Property(review => review.IdUser).IsRequired();
            builder.Property(review => review.IdCourse).IsRequired();
            builder.Property(review => review.Rait).IsRequired();
            builder.Property(review => review.CreatedAt).IsRequired();
            builder.Property(review => review.Text).IsRequired().HasMaxLength(300);

            builder.HasOne(user => user.User)
                .WithMany(reviews => reviews.Reviews)
                .HasForeignKey(k => k.IdUser);

            builder.HasOne(course => course.Course)
               .WithMany(reviews => reviews.Reviews)
               .HasForeignKey(k => k.IdCourse);


        }
    }
}
