using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Materials.CreateMaterial
{
    public class CreateMaterialCommandHandler(ICoursesDbContext context) : IRequestHandler<CreateMaterialCommand, Guid>
    {
        public async Task<Guid> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
        {

            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entity = await context.Modules
                .Include(u => u.Course)
                .FirstOrDefaultAsync(u => u.Id == request.ModuleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Material), request.ModuleId);

            if (roleUser.Name == "Admin" || (roleUser.Name == "Couch" && entity.Course.UserId == currentUser.Id))
            {
                int order = 0;
                if (context.Materials.Any(m => m.ModuleId == request.ModuleId))
                    order = context.Materials.OrderBy(m => m.Order).LastAsync(cancellationToken).Result.Order;

                var matherial = new Material
                {
                    Id = Guid.NewGuid(),
                    ModuleId = request.ModuleId,
                    Title = request.Title,
                    Description = request.Description,
                    Order = ++order
                };
                if (entity.Course.Status == "Published")
                    entity.Course.UpdateAt = DateTime.UtcNow;
                await context.Materials.AddAsync(matherial,cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return matherial.Id;
            }
            throw new AccessException();
        }
    }
}
