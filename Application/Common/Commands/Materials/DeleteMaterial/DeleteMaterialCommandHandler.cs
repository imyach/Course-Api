using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Materials.DeleteMaterial
{
    public class DeleteMaterialCommandHandler(ICoursesDbContext context) : IRequestHandler<DeleteMaterialCommand>
    {
        public async Task<Unit> Handle(DeleteMaterialCommand request, CancellationToken cancellationToken)
        {

            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entity = await context.Materials
             .Include(u => u.Module)
                .ThenInclude(m=>m.Course)
             .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken)
             ?? throw new NotFoundException(nameof(Material), request.Id);

            if (roleUser.Name == "Admin" || (roleUser.Name == "Couch" && currentUser.Id == entity.Module.Course.UserId))
            {
                if (entity.Module.Course.Status == "Published")
                    entity.Module.Course.UpdateAt = DateTime.UtcNow;
                context.Materials.Remove(entity);
                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            throw new AccessException();
        }
    }
}
