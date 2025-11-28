using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Modules.DeleteModule
{
    public class DeleteModuleCommandHandler(ICoursesDbContext context) : IRequestHandler<DeleteModuleCommand>
    {
        public async Task<Unit> Handle(DeleteModuleCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.Modules.FindAsync([request.Id], cancellationToken);
            if(entity == null || entity.CurrentUserId != request.CurrentUserId)
            {
                throw new NotFoundException(nameof(Module), request.Id);
            }

            context.Modules.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
