using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.EntityFrameworkConfiguration
{
    public class ReviewsConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(review => review.Id);
            builder.HasIndex(review => review.Id).IsUnique();
            builder.Property(review => review.Id).HasMaxLength(250);
            builder.Property(review => review.UserId).IsRequired();
            builder.Property(review => review.CourseId).IsRequired();
            builder.Property(review => review.Rait).IsRequired();
            builder.Property(review => review.CreatedAt).IsRequired();
            builder.Property(review => review.Text).IsRequired().HasMaxLength(300);

            builder.HasOne(user => user.User)
                .WithMany(reviews => reviews.Reviews)
                .HasForeignKey(k => k.UserId);

            builder.HasOne(course => course.Course)
               .WithMany(reviews => reviews.Reviews)
               .HasForeignKey(k => k.CourseId);


        }
    }
}
