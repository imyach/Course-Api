using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Courses.DeleteCourse
{
    public class DeleteCourseCommandHandler(ICoursesDbContext context) : IRequestHandler<DeleteCourseCommand>
    {
        public async Task<Unit> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entity = await context.Courses.FindAsync([request.Id], cancellationToken)
                ?? throw new NotFoundException(nameof(Course), request.Id);

            if (roleUser.RoleName == "Admin" || (entity.UserId == currentUser.Id && roleUser.RoleName == "Couch"))
            {
                entity.Status = "Archived";
                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            throw new AccessException();
        }
    }
}
