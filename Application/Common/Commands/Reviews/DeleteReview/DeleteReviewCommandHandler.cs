using Application.Common.Commands.Matherials.DeleteMatherial;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Rewies.DeleteReview
{
    public class DeleteReviewCommandHandler(ICoursesDbContext context) : IRequestHandler<DeleteReviewCommand>
    {
        public async Task<Unit> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.Reviews.FindAsync([request.Id], cancellationToken);

            if (entity == null || entity.CurrentUserId != request.CurrentUserId)
            {
                throw new NotFoundException(nameof(Review), request.Id);
            }

            context.Reviews.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}