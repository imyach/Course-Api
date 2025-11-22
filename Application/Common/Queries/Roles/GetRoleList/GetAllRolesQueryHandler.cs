using Application.Common.Dtos.Roles;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
            var rolesQuery = await context.Roles
                .Where(r=> r.UserId == request.UserId)
                .ProjectTo<RoleLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new RoleListVm { RoleListDto = rolesQuery };
        }
    }
}
