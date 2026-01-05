using Application.Common.Dtos;
using Application.Common.Dtos.Courses;
using Application.Common.Dtos.Modules;
using Application.Common.Dtos.Reviews;
using Application.Common.Queries.Modules.GetModuleList;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Reviews.GetReviewList
{
    public class GetAllReviewQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetAllReviewQuery, object[]>
    {
        public async Task<object[]> Handle(GetAllReviewQuery request, CancellationToken cancellationToken)
        {

            var query = context.Reviews
                .Include(m => m.Course)
                .Include(m => m.User)
                .Where(r => r.CourseId == request.IdCourse)
                .AsQueryable();

            var totalItems = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            var reviews = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<ReviewLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return [ new ReviewListVm { Reviews = reviews },
                new PagerInfoDto{ TotalItems = totalItems,
                TotalPages = totalPages,
                PageSize = request.PageSize,
                PageNumber = request.PageNumber}
            ];
        }
    }
}
