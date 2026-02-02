using Application.Common.Dtos.ProgressModules;
using Application.Common.Dtos.ProgressUsers;
using Application.Common.Exceptions;
using Application.Common.Queries.Modules.GetModuleList;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.ProgressModules.GetProgressModulesList
{
    public class GetAllProgressModuleQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetAllProgressModuleQuery, ProgressModuleListVm>
    {
        public async Task<ProgressModuleListVm> Handle(GetAllProgressModuleQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var query = await context.ProgressModules
                .Where(m => m.UserId == request.UserId && m.ProgressUserId == request.ProgressUserId)
                .Include(pm => pm.Module)
                .Include(m => m.ProgressUser)
                .Include(m => m.User)
                .OrderBy(pm=>pm.Module.Order)
                .ProjectTo<ProgressModuleLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new ProgressModuleListVm { ProgressModule = query };
        }
    }
}
