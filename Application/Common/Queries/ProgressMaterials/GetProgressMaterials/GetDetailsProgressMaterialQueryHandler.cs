using Application.Common.Dtos.ProgressMaterials;
using Application.Common.Dtos.ProgressModules;
using Application.Common.Exceptions;
using Application.Common.Queries.ProgressModules.GetProgressModules;
using Application.Interfaces;
using AutoMapper;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.ProgressMaterials.GetProgressMaterials
{
    public class GetDetailsProgressMaterialQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetDetailsProgressMaterialQuery, ProgressMaterialLookupDto>
    {
        public async Task<ProgressMaterialLookupDto> Handle(GetDetailsProgressMaterialQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entities = await context.ProgressMaterials
            .Include(pm => pm.ProgressModule)
            .Include(pm => pm.Material)
            .Include(pm => pm.User)
            .ToListAsync(cancellationToken);

            if (roleUser.Name == "Admin" || roleUser.Name == "Couch")
            {

                var entityA = entities.FirstOrDefault(r => r.Id == request.Id)
                    ?? throw new NotFoundException(nameof(ProgressMaterial), request.Id);

                return mapper.Map<ProgressMaterialLookupDto>(entityA);
            }

            var entity = entities.Where(e => e.UserId == currentUser.Id)
                .FirstOrDefault(r => r.Id == request.Id)
                    ?? throw new NotFoundException(nameof(ProgressMaterial), request.Id);

            return mapper.Map<ProgressMaterialLookupDto>(entity);
        }
    }
}
