using Application.Common.Commands.Rewies.CreateReview;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressUsers.CreateProgressUser
{
    public class CreateProgressUserCommandHandler(ICoursesDbContext context) : IRequestHandler<CreateProgressUserCommand, Guid>
    {
        public async Task<Guid> Handle(CreateProgressUserCommand request, CancellationToken cancellationToken)
        {
            var progressUser = new ProgressUser
            {
                Id = Guid.NewGuid(),
                CourseId = request.CourseId,
                UserId = request.CurrentUserId,
                Status = "В прохождении",
                StartedAt = DateTime.UtcNow,
                FineshedAt = null,
            };

            await context.ProgressUsers.AddAsync(progressUser, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return progressUser.Id;
        }
    }
}
