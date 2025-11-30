using Application.Common.Commands.Rewies.DeleteReview;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressUsers.DeleteProgressUser
{
    public class DeleteProgressUserCommandHandler(ICoursesDbContext context) : IRequestHandler<DeleteProgressUserCommand>
    {
        public async Task<Unit> Handle(DeleteProgressUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.ProgressUsers.FindAsync([request.Id], cancellationToken);

            if (entity == null || entity.CurrentUserId != request.CurrentUserId)
            {
                throw new NotFoundException(nameof(ProgressUser), request.Id);
            }

            context.ProgressUsers.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}