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
            var entity = await context.Courses.FindAsync([request.Id], cancellationToken);
            if (entity == null || entity.CurrentUserId != request.CurrentUserId) 
            {
                throw new NotFoundException(nameof(Course), request.Id);
            }
            context.Courses.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
