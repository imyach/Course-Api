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

            var entity = await context.Materials
            .Include(u => u.Module)
                .ThenInclude(m => m.Course)
            .FirstOrDefaultAsync(u => u.Id == request.MaterialId, cancellationToken)
            ?? throw new NotFoundException(nameof(Test), request.MaterialId);

            if (roleUser.Name == "Admin" || (roleUser.Name == "Couch" && entity.Module.Course.UserId == currentUser.Id))
            {
                int order = 0;
                if (context.Tests.Any(m => m.MaterialId == request.MaterialId))
                    order = context.Tests.OrderBy(m => m.Order).LastAsync(cancellationToken).Result.Order;

                var test = new Test
                {
                    Id = Guid.NewGuid(),
                    MaterialId = request.MaterialId,
                    Title = request.Title,
                    Description = request.Description,
                    Order = ++order
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
