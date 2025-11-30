using Application.Common.Dtos.Users;
using Application.Common.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Users.GetUser
{
    public class GetDetailsUserQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetDetailsUserQuery, UserLookupDto>
    {
        public async Task<UserLookupDto> Handle(GetDetailsUserQuery request, CancellationToken cancellationToken)
        {
            var entity = await context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity == null || entity.CurrentUserId != request.CurrentUserId ) 
            {
                throw new NotFoundException(nameof(User), request.Id);
            }
            return mapper.Map<UserLookupDto>(entity);
        }
    }
}
