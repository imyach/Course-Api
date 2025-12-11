using Application.Common.Dtos.Modules;
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

namespace Application.Common.Queries.Modules.GetModule
{
    public class GetDetailsModuleQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetDetailsModuleQuery, ModuleLookupDto>
    {
        public async Task<ModuleLookupDto> Handle(GetDetailsModuleQuery request, CancellationToken cancellationToken)
        {
            var entity = await context.Modules
                .Include(m => m.Course)

                .Include(m => m.Course)
                    .ThenInclude(c => c.User)
                    .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken) 
                ?? throw new NotFoundException(nameof(Module), request.Id);
            return mapper.Map<ModuleLookupDto>(entity);
        }
    }
}
