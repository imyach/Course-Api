using Application.Common.Commands.ProgressUsers.CreateProgressUser;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressModules.CreateProgressModules
{
    public class CreateProgressModuleCommandHandler(ICoursesDbContext context) : IRequestHandler<CreateProgressModuleCommand, Guid>
    {
        public async Task<Guid> Handle(CreateProgressModuleCommand request, CancellationToken cancellationToken)
        {
            var progressModel = new ProgressModule
            {
                Id = Guid.NewGuid(),
                ModuleId = request.ModuleId,
                ProgressUserId = request.ProgressUserId,
                UserId = request.CurrentUserId,
                Status = "Не начат",
                StartedAt = null
            };

            await context.ProgressModules.AddAsync(progressModel, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return progressModel.Id;
        }
    }
}
