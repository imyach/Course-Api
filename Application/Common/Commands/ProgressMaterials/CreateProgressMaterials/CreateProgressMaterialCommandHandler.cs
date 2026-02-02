using Application.Common.Commands.ProgressModules.CreateProgressModules;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressMaterials.CreateProgressMaterials
{
    public class CreateProgressMaterialCommandHandler(ICoursesDbContext context) : IRequestHandler<CreateProgressMaterialCommand, Guid>
    {
        public async Task<Guid> Handle(CreateProgressMaterialCommand request, CancellationToken cancellationToken)
        {
            var progressMaterial = new ProgressMaterial
            {
                Id = Guid.NewGuid(),
                MaterialId = request.MaterialId,
                ProgressModuleId = request.ProgressModuleId,
                UserId = request.CurrentUserId,
                Status = "Не начат",
                StartedAt = null
            };


            await context.ProgressMaterials.AddAsync(progressMaterial, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return progressMaterial.Id;
        }
    }
}
