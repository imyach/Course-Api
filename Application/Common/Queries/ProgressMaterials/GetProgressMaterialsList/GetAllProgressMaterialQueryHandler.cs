using Application.Common.Dtos.Materials;
using Application.Common.Dtos.ProgressMaterials;
using Application.Common.Dtos.ProgressModules;
using Application.Common.Exceptions;
using Application.Common.Queries.ProgressModules.GetProgressModulesList;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.ProgressMaterials.GetProgressMaterialsList
{
    public class GetAllProgressMaterialQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetAllProgressMaterialQuery, ProgressMaterialListVm>
    {
        public async Task<ProgressMaterialListVm> Handle(GetAllProgressMaterialQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var query = await context.ProgressMaterials
                .Where(m => m.UserId == request.CurrentUserId && m.ProgressModuleId == request.ProgressModuleId)
                .Include(pm => pm.Material)
                .Include(m => m.ProgressModule)
                .Include(m => m.User)
                .OrderBy(pm => pm.Material.Order)
                .ProjectTo<ProgressMaterialLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new ProgressMaterialListVm { ProgressMaterials = query };
        }
    }
}
