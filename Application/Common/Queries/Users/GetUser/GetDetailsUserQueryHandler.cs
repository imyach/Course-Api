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
    public class GetDetailsUserQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetDetailsUserQuery, UserDetailsVm>
    {
        public async Task<UserDetailsVm> Handle(GetDetailsUserQuery request, CancellationToken cancellationToken)
        {
            var entity = await context.Users.
                FirstOrDefaultAsync(x => x.Id == request.Id);
            if (entity == null || entity.UserId != request.Id) 
            {
                throw new NotFoundException(nameof(User), request.Id);
            }
            return mapper.Map<UserDetailsVm>(entity);
        }
    }
}
