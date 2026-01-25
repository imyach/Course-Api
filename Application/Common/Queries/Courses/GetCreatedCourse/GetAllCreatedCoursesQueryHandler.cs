using Application.Common.Dtos;
using Application.Common.Dtos.Courses;
using Application.Common.Queries.Courses.GetCourseList;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Courses.GetCreatedCourse
{
    internal class GetAllCreatedCoursesQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetAllCreatedCoursesQuery, object[]>
    {
        public async Task<object[]> Handle(GetAllCreatedCoursesQuery request, CancellationToken cancellationToken)
        {
            var query = context.Courses
                .Include(c => c.User)
                .Where(c => c.Status == "Draft" && c.User.Id == request.CurrentUserId)
                .AsQueryable();


            var totalItems = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            var courses = await query
                .OrderBy(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<CourseLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return
            [ new CourseListVm { Courses = courses },
              new PagerInfoDto{ TotalItems = totalItems,
                TotalPages = totalPages,
                PageSize = request.PageSize,
                PageNumber = request.PageNumber}
            ];
        }
    }
}
