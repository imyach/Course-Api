using Application.Common.Dtos.Courses;
using Application.Common.Exceptions;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Courses.GetCourse
{
    public class GetDetailsCourseQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetDetailsCourseQuery, CourseLookupDto>
    {
        public async Task<CourseLookupDto> Handle(GetDetailsCourseQuery request, CancellationToken cancellationToken)
        {
            var entity = await context.Courses
                .Include(c => c.User)
                    .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(c=> c.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Course), request.Id);
            return mapper.Map<CourseLookupDto>(entity);
        }
    }
}
