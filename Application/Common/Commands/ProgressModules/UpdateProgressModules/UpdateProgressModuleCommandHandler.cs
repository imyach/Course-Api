using Application.Common.Commands.ProgressUsers.UpdateProgressUser;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressModules.UpdateProgressModules
{
    public class UpdateProgressModuleCommandHandler(ICoursesDbContext context) : IRequestHandler<UpdateProgressModuleCommand>
    {
        public async Task<Unit> Handle(UpdateProgressModuleCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.ProgressModules.FindAsync([request.Id], cancellationToken)
               ?? throw new NotFoundException(nameof(ProgressUser), request.Id);

            if (entity.UserId != request.CurrentUserId)
                throw new AccessException();

            if (entity.Status != "Завершен")
                entity.Status = request.Status;

            if(entity.StartedAt is null)
                entity.StartedAt = request.StartedAt;

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
