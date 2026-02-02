using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Tests.UpdateTest
{
    public class UpdateTestCommandHandler(ICoursesDbContext context) : IRequestHandler<UpdateTestCommand>
    {
        public async Task<Unit> Handle(UpdateTestCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entity = await context.Tests
                .Include(u => u.Material)
                    .ThenInclude(m => m.Module)
                    .ThenInclude(m => m.Course)
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Test), request.Id);

            if (roleUser.RoleName == "Admin" || (roleUser.RoleName == "Couch" && currentUser.Id == entity.Material.Module.Course.UserId))
            {
                entity.Title = request.Title;
                entity.Description = request.Description; 

                if (entity.Material.Module.Course.Status == "Published")
                    entity.Material.Module.Course.UpdateAt = DateTime.UtcNow;

                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            throw new AccessException();
        }
    }
}
