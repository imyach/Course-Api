using Application.Common.Commands.ProgressUsers.DeleteProgressUser;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressModules.DeleteProgressModules
{
    public class DeleteProgressModuleCommandHandler(ICoursesDbContext context) : IRequestHandler<DeleteProgressModuleCommand>
    {
        public async Task<Unit> Handle(DeleteProgressModuleCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entity = await context.ProgressModules.FindAsync([request.Id], cancellationToken)
                ?? throw new NotFoundException(nameof(ProgressModule), request.Id);
            if (roleUser.RoleName == "Admin" || (entity.UserId == currentUser.Id))
            {
                context.ProgressModules.Remove(entity);
                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            throw new AccessException();
        }
    }
}
