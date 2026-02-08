using Application.Common.Commands.ProgressModules.UpdateProgressModules;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressMaterials.UpdateProgressMaterials
{
    public class UpdateProgressMaterialCommandHandler(ICoursesDbContext context) : IRequestHandler<UpdateProgressMaterialCommand>
    {
        public async Task<Unit> Handle(UpdateProgressMaterialCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.ProgressMaterials.FindAsync([request.Id], cancellationToken)
               ?? throw new NotFoundException(nameof(ProgressMaterial), request.Id);

            if (entity.UserId != request.CurrentUserId)
                throw new AccessException();

            entity.Status = request.Status;

            if (entity.StartedAt is null)
                entity.StartedAt = request.StartedAt;

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
