using Application.Common.Commands.ProgressModules.DeleteProgressModules;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressMaterials.DeleteProgressMaterials
{
    public class DeleteProgressMaterialCommandHandler(ICoursesDbContext context) : IRequestHandler<DeleteProgressMaterialCommand>
    {
        public async Task<Unit> Handle(DeleteProgressMaterialCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entity = await context.ProgressMaterials.FindAsync([request.Id], cancellationToken)
                ?? throw new NotFoundException(nameof(ProgressMaterial), request.Id);
            if (roleUser.RoleName == "Admin" || (entity.UserId == currentUser.Id))
            {
                context.ProgressMaterials.Remove(entity);
                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            throw new AccessException();
        }
    }
}
