using Application.Common.Dtos.Users;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Users.GetUserByEmail
{
    public class GetUserByEmailCommandHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetUserByEmailCommand, UsersListVm>
    {
        public async Task<UsersListVm> Handle(GetUserByEmailCommand request, CancellationToken cancellationToken)
        {
            var usersList = await context.Users
                .Include(u => u.Role)
                .Where(x => x.Email == request.Email)
                .ProjectTo<UserLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync( cancellationToken);

            return new UsersListVm { Users = usersList };
        }
    }
}
