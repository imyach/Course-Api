using Application.Common.Dtos.ProgressUsers;
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

namespace Application.Common.Queries.ProgressUsers.GetProgressUser
{
    public class GetDetailsProgressUserQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetDetailsProgressUserQuery, ProgressUserLookupDto>
    {
        public async Task<ProgressUserLookupDto> Handle(GetDetailsProgressUserQuery request, CancellationToken cancellationToken)
        {
            var entity = await context.ProgressUsers
                 .Include(r => r.Course)
                     .ThenInclude(c => c.User)
                     .ThenInclude(u => u.Role)
                 .Include(r => r.User)
                     .ThenInclude(u => u.Role)
                 .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
            if (entity == null || request.CurrentUserId != entity.CurrentUserId)
            {
                throw new NotFoundException(nameof(ProgressUser), request.Id);
            }

            return mapper.Map<ProgressUserLookupDto>(entity);
        }
    }
}
