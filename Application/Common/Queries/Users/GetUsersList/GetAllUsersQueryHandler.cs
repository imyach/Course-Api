using Application.Common.Dtos.Users;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Users.GetUsersList
{
    public class GetAllUsersQueryHandler(IMapper mapper, ICoursesDbContext context) : IRequestHandler<GetAllUsersQuery, UsersListVm>
    {
        public async Task<UsersListVm> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var usersQuery = await context.Users
                .Where(u => u.UserId == request.UserId)
                .ProjectTo<UserLooupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new UsersListVm { UsersListDto =  usersQuery };
        }
    }
}
