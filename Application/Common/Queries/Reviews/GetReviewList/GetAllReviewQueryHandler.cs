using Application.Common.Dtos.Modules;
using Application.Common.Dtos.Reviews;
using Application.Common.Queries.Modules.GetModuleList;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Reviews.GetReviewList
{
    public class GetAllReviewQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetAllReviewQuery, ReviewListVm>
    {
        public async Task<ReviewListVm> Handle(GetAllReviewQuery request, CancellationToken cancellationToken)
        {
            var reviewQuery = await context.Reviews
                .Include(m => m.Course)
                .Include(m => m.User)
                .Where(m => m.CurrentUserId == request.CurrentUserId)
                .ProjectTo<ReviewLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new ReviewListVm { Reviews = reviewQuery };
        }
    }
}
