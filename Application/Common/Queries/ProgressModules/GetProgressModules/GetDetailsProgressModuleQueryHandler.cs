using Application.Common.Dtos.ProgressModules;
using Application.Common.Dtos.ProgressUsers;
using Application.Common.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.ProgressModules.GetProgressModules
{
    public class GetDetailsProgressModuleQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetDetailsProgressModuleQuery, ProgressModuleLookupDto>
    {
        public async Task<ProgressModuleLookupDto> Handle(GetDetailsProgressModuleQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entities = await context.ProgressModules
            .Include(pm => pm.ProgressUser)
            .Include(pm => pm.Module)
            .Include(pm => pm.User)
            .ToListAsync(cancellationToken);

            if (roleUser.RoleName == "Admin" || roleUser.RoleName == "Couch")
            {

                var entityA = entities.FirstOrDefault(r => r.Id == request.Id)
                    ?? throw new NotFoundException(nameof(ProgressModule), request.Id);

                return mapper.Map<ProgressModuleLookupDto>(entityA);
            }

            var entity = entities.Where(e => e.UserId == currentUser.Id)
                .FirstOrDefault(r => r.Id == request.Id)
                    ?? throw new NotFoundException(nameof(ProgressModule), request.Id);

            return mapper.Map<ProgressModuleLookupDto>(entity);
        }
    }
}
