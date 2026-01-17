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

            var query = context.Courses
                .Include(c => c.User).AsQueryable();

            if(!string.IsNullOrEmpty(request.SearchText))
            {
                query = query.Where(x=>x.Title.ToLower().Contains(request.SearchText.ToLower()) 
                    || x.Description.ToLower().Contains(request.SearchText.ToLower())
                    || x.User.NameUser.ToLower().Contains(request.SearchText.ToLower()));
            }


            var totalItems = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            var courses = await query
                .OrderBy(x=>x.Id)
                .Skip((request.PageNumber-1) * request.PageSize)
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
