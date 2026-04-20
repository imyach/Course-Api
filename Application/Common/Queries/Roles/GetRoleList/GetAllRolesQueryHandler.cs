using Application.Common.Dtos.Roles;
using Application.Common.Exceptions;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Roles.GetRoleList
{
    public class GetAllRolesQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetAllRolesQuery, RoleListVm>
    {
        public async Task<RoleListVm> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);
            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            if (roleUser.Name == "Admin")
            {
                var rolesQuery = await context.Roles
                .ProjectTo<RoleLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

                return new RoleListVm { Roles = rolesQuery };
            }
            throw new AccessException();
        }
    }
}
