using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressUsers.UpdateProgressUser
{
    public class UpdateProgressUserCommandHandler(ICoursesDbContext context) : IRequestHandler<UpdateProgressUserCommand>
    {
        public async Task<Unit> Handle(UpdateProgressUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.ProgressUsers.FindAsync([request.Id],cancellationToken)
                ?? throw new NotFoundException(nameof(ProgressUser), request.Id);

            if (entity.UserId != request.CurrentUserId)
                throw new AccessException();

            entity.Status = request.Status;
            entity.FineshedAt = request.FinishedAt;

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
