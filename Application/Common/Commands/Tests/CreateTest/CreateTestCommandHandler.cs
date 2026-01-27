using Application.Common.Dtos.Tests;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Tests.CreateTest
{
    public class CreateTestCommandHandler(ICoursesDbContext context) : IRequestHandler<CreateTestCommand, Guid>
    {
        public async Task<Guid> Handle(CreateTestCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entity = await context.Matherials
            .Include(u => u.Module)
                .ThenInclude(m => m.Course)
            .FirstOrDefaultAsync(u => u.Id == request.MatherialId, cancellationToken)
            ?? throw new NotFoundException(nameof(Test), request.MatherialId);

            if (roleUser.RoleName == "Admin" || (roleUser.RoleName == "Couch" && entity.Module.Course.UserId == currentUser.Id))
            {
                var test = new Test
                {
                    Id = Guid.NewGuid(),
                    MatherialId = request.MatherialId,
                    Title = request.Title,
                    Description = request.Description,
                };

                if (entity.Module.Course.Status == "Published")
                    entity.Module.Course.UpdateAt = DateTime.UtcNow;
                await context.Tests.AddAsync(test, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return test.Id;
            }
            throw new AccessException();
        }
    }
}
