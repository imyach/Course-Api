using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Tests.DeleteTest
{
    public class DeleteTestCommandHandler(ICoursesDbContext context) : IRequestHandler<DeleteTestCommand>
    {
        public async Task<Unit> Handle(DeleteTestCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entity = await context.Tests
            .Include(u => u.Material)
                .ThenInclude(m=>m.Module)
                .ThenInclude(m => m.Course)
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Test), request.Id);

            if (roleUser.Name == "Admin" || (roleUser.Name == "Couch" && currentUser.Id == entity.Material.Module.Course.UserId))
            {
                if (entity.Material.Module.Course.Status == "Published")
                    entity.Material.Module.Course.UpdateAt = DateTime.UtcNow;
                context.Tests.Remove(entity);
                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            throw new AccessException();
        }
    }
}
