using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Modules.DeleteModule
{
    public class DeleteModuleCommandHandler(ICoursesDbContext context) : IRequestHandler<DeleteModuleCommand>
    {
        public async Task<Unit> Handle(DeleteModuleCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entity = await context.Modules.FindAsync([request.Id], cancellationToken) 
                ?? throw new NotFoundException(nameof(Module), request.Id);

            if (roleUser.RoleName == "Admin" || (roleUser.RoleName == "Couch" && currentUser.Id == entity.Course.UserId))
            {
                entity.Course.UpdateAt = DateTime.Now;
                context.Modules.Remove(entity);
                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            throw new AccessException();
            
        }
    }
}
