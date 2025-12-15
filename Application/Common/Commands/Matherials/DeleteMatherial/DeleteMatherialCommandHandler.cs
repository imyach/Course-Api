using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Matherials.DeleteMatherial
{
    public class DeleteMatherialCommandHandler(ICoursesDbContext context) : IRequestHandler<DeleteMatherialCommand>
    {
        public async Task<Unit> Handle(DeleteMatherialCommand request, CancellationToken cancellationToken)
        {

            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entity = await context.Matherials.FindAsync([request.Id], cancellationToken)
                ?? throw new NotFoundException(nameof(Matherial), request.Id);

            if (roleUser.RoleName == "Admin" || (roleUser.RoleName == "Couch" && currentUser.Id == entity.Module.Course.UserId))
            {

                entity.Module.Course.UpdateAt = DateTime.UtcNow;
                context.Matherials.Remove(entity);
                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            throw new AccessException();
        }
    }
}
