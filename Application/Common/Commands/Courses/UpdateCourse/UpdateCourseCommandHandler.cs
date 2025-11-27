using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Courses.UpdateCourse
{
    public class UpdateCourseCommandHandler(ICoursesDbContext context) : IRequestHandler<UpdateCourseCommand>
    {
        public async Task<Unit> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.Courses.FindAsync([request.Id], cancellationToken);
            if(entity == null || entity.CurrentUserId != request.CurrentUserId)
            {
                throw new NotFoundException(nameof(Course),request.Id);
            }

            entity.UpdateAt = DateTime.Now;
            entity.Title = request.Title; 
            entity.Description = request.Description;   
            entity.Rait = request.Rait;
            entity.UserId = request.UserId;
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
