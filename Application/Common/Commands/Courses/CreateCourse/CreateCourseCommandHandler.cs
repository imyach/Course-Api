using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Courses.CreateCourse
{
    public class CreateCourseCommandHandler(ICoursesDbContext context) : IRequestHandler<CreateCourseCommand, Guid>
    {
        public async Task<Guid> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);
            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            if (roleUser.RoleName != "Student")
            {
                var course = new Course
                {
                    Id = Guid.NewGuid(),
                    Title = request.Title,
                    Description = request.Description,
                    CreatedAt = DateTime.Now,
                    Rait = request.Rait,
                    UserId = request.CurrentUserId,
                    UpdateAt = null
                };

                await context.Courses.AddAsync(course, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return course.Id;
            }
            throw new AccessException();
        }
    }
}
