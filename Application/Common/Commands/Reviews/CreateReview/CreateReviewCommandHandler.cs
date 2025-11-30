using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Rewies.CreateReview
{
    public class CreateReviewCommandHandler(ICoursesDbContext context) : IRequestHandler<CreateReviewCommand, Guid>
    {
        public async Task<Guid> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = new Review
            {
                Id = Guid.NewGuid(),
                CourseId = request.CourseId,
                UserId = request.UserId,
                Rait = request.Rait,
                Text = request.Text,
                CreatedAt = DateTime.Now
            };

            await context.Reviews.AddAsync(review, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return review.Id;
        }
    }
}
