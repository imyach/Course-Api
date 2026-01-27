using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Matherials.CreateMatherial
{
    public class CreateMatherialCommandHandler(ICoursesDbContext context) : IRequestHandler<CreateMatherialCommand, Guid>
    {
        public async Task<Guid> Handle(CreateMatherialCommand request, CancellationToken cancellationToken)
        {

            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entity = await context.Modules
                .Include(u => u.Course)
                .FirstOrDefaultAsync(u => u.Id == request.ModuleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Matherial), request.ModuleId);

            if (roleUser.RoleName == "Admin" || (roleUser.RoleName == "Couch" && entity.Course.UserId == currentUser.Id))
            {
                int order = 0;
                if (context.Matherials.Any(m => m.ModuleId == request.ModuleId))
                    order = context.Matherials.OrderBy(m => m.Order).LastAsync(cancellationToken).Result.Order;

                var matherial = new Matherial
                {
                    Id = Guid.NewGuid(),
                    ModuleId = request.ModuleId,
                    Title = request.Title,
                    Description = request.Description,
                    Order = ++order
                };
                if (entity.Course.Status == "Published")
                    entity.Course.UpdateAt = DateTime.UtcNow;
                await context.Matherials.AddAsync(matherial,cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return matherial.Id;
            }
            throw new AccessException();
        }
    }
}
