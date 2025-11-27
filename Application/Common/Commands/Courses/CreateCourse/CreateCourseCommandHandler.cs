using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Courses.CreateCourse
{
    public class CreateCourseCommandHandler(ICoursesDbContext context) : IRequestHandler<CreateCourseCommand, Guid>
    {
        public async Task<Guid> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var course = new Course
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                CreatedAt = DateTime.Now,
                Rait = request.Rait,
                UserId = request.UserId,
                UpdateAt = null
            };

            await context.Courses.AddAsync(course,cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return course.Id;
        }
    }
}
