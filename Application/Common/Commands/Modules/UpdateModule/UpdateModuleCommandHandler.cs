using Application.Common.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Modules.UpdateModule
{
    public class UpdateModuleCommandHandler(ICoursesDbContext context) : IRequestHandler<UpdateModuleCommand>
    {
        public async Task<Unit> Handle(UpdateModuleCommand request, CancellationToken cancellationToken)
        {

            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);
            
            var entity = await context.Modules
               .Include(u => u.Course)
               .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken)
               ?? throw new NotFoundException(nameof(Module), request.Id);

            if (roleUser.RoleName == "Admin" || (roleUser.RoleName == "Couch" && currentUser.Id == entity.Course.UserId))
            {
                if(!string.IsNullOrEmpty(request.Title))
                    entity.Title = request.Title;
                    entity.Description = request.Description;
                if (entity.Course.Status == "Published")
                    entity.Course.UpdateAt = DateTime.UtcNow;
                await context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
            throw new AccessException();
        }
    }
}
