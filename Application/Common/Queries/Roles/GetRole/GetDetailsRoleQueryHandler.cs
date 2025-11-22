using Application.Common.Dtos.Roles;
using Application.Common.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Roles.GetRole
{
    public class GetDetailsRoleQueryHandler(ICoursesDbContext context, IMapper mapper) 
        : IRequestHandler<GetDetailsRoleQuery, RoleDetailsVm>
    {
        public async Task<RoleDetailsVm> Handle(GetDetailsRoleQuery request, CancellationToken cancellationToken)
        {
            var entity = await context.Roles
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            if (entity == null || entity.UserId != request.UserId )
            {
                throw new NotFoundException(nameof(Role), request.Id);
            }

            return mapper.Map<RoleDetailsVm>(entity);
        }
    }
}
