using Application.Common.Dtos.Modules;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Modules.GetModuleList
{
    public class GetAllModuleQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetAllModuleQuery, ModuleListVm>
    {
        public async Task<ModuleListVm> Handle(GetAllModuleQuery request, CancellationToken cancellationToken)
        {
            var moduleQuery = await context.Modules
                .Include(m=> m.Course)
                .Where(m=>m.CourseId == request.CourseId)
                .OrderBy(m=>m.Order)
                .ProjectTo<ModuleLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new ModuleListVm { Modules = moduleQuery };
        }
    }
}
