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

            var course = await context.Courses.FirstOrDefaultAsync(r => r.Id == request.CourseId, cancellationToken)
                ?? throw new NotFoundException(nameof(Course), request.CourseId);

            if (roleUser.RoleName == "Admin" || (roleUser.RoleName == "Couch" && course.UserId == currentUser.Id))
            {

                var module = new Module
                {
                    Id = Guid.NewGuid(),
                    CourseId = request.CourseId,
                    Title = request.Title,
                    Description = request.Description,
                    Order = request.Order,
                };

                course.UpdateAt = DateTime.UtcNow;
                await context.Modules.AddAsync(module, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return module.Id;
            }
            throw new AccessException();
        }
    }
}
