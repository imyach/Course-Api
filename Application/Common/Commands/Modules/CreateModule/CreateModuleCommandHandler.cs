using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Modules.CreateModule
{
    public class CreateModuleCommandhandler(ICoursesDbContext context) : IRequestHandler<CreateModuleCommand, Guid>
    {
        public async Task<Guid> Handle(CreateModuleCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entity = await context.Courses
               .FirstOrDefaultAsync(u => u.Id == request.CourseId, cancellationToken)
               ?? throw new NotFoundException(nameof(Course), request.CourseId);

            if (roleUser.Name == "Admin" || (roleUser.Name == "Couch" && entity.UserId == currentUser.Id))
            {
                int order = 0;
                if (context.Modules.Any(m=>m.CourseId == request.CourseId))
                    order = context.Modules.OrderBy(m=>m.Order).LastAsync(cancellationToken).Result.Order;


                var module = new Module
                {
                    Id = Guid.NewGuid(),
                    CourseId = request.CourseId,
                    Title = request.Title,
                    Description = request.Description,
                    Order = ++order
                };

                if(entity.Status == "Published")
                    entity.UpdateAt = DateTime.UtcNow;
                await context.Modules.AddAsync(module, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return module.Id;
            }
            throw new AccessException();
        }
    }
}
