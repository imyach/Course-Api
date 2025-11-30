using Application.Common.Dtos.Modules;
using Application.Common.Dtos.Reviews;
using Application.Common.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Reviews.GetReview
{
    public class GetDetailsReviewQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetDetailsReviewQuery, ReviewLookupDto>
    {
        public async Task<ReviewLookupDto> Handle(GetDetailsReviewQuery request, CancellationToken cancellationToken)
        {
            var entity = await context.Reviews
                .Include(r => r.Course)
                    .ThenInclude(c => c.User)
                    .ThenInclude(u => u.Role)
                .Include(r => r.User)
                    .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
            if (entity == null || request.CurrentUserId != entity.CurrentUserId)
            {
                throw new NotFoundException(nameof(Review), request.Id);
            }

            return mapper.Map<ReviewLookupDto>(entity);
        }
    }
}
