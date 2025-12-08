using Application.Common.Dtos.Courses;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Courses.GetCourseList
{
    public class GetAllCourseQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetAllCourseQuery, CourseListVm>
    {
        public async Task<CourseListVm> Handle(GetAllCourseQuery request, CancellationToken cancellationToken)
        { 
            var courseQuery = await context.Courses
                .Include(c => c.User)
                .ProjectTo<CourseLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new CourseListVm { Courses = courseQuery };
        }
    }
}
