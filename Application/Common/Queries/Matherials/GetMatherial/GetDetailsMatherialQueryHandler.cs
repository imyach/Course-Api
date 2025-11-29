using Application.Common.Dtos.Matherials;
using Application.Common.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Matherials.GetMatherial
{
    public class GetDetailsMatherialQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetDetailsMatherialQuery, MatherialLookupDto>
    {
        public async Task<MatherialLookupDto> Handle(GetDetailsMatherialQuery request, CancellationToken cancellationToken)
        {
            var matherial = await context.Matherials
                .Include(m => m.Module)
                    .ThenInclude(m => m.Course)
                    .ThenInclude(c => c.User)
                    .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);
            if (matherial == null || request.CurrentUserId != matherial.CurrentUserId)
            {
                throw new NotFoundException(nameof(Matherial), request.Id);
            }
            return mapper.Map<MatherialLookupDto>(matherial);
        }
    }
}
