using Application.Common.Dtos;
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
    public class GetAllCourseQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetAllCourseQuery, object[]>
    {
        public async Task<object[]> Handle(GetAllCourseQuery request, CancellationToken cancellationToken)
        {
            var totalItems = await context.Courses.CountAsync(cancellationToken);

            var courseQuery = await context.Courses
                .Include(c => c.User)
                .OrderBy(x=>x.Id)
                .Skip((request.PageNumber-1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<CourseLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);


            return
            [ new CourseListVm { Courses = courseQuery },
              new PagerInfoDto{ TotalItems = totalItems,
                TotalPages = totalPages,    
                PageSize = request.PageSize,
                PageNumber = request.PageNumber}
            ];

        }
    }
}
