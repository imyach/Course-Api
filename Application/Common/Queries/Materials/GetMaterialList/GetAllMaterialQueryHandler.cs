using Application.Common.Dtos.Materials;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Materials.GetMaterialList
{
    public class GetAllMaterialQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetAllMaterialQuery, MaterialListVm>
    {
        public async Task<MaterialListVm> Handle(GetAllMaterialQuery request, CancellationToken cancellationToken)
        {
            var matherialsQuery = await context.Materials
                .Include(m => m.Module)
                .Where(m=>m.ModuleId == request.ModuleId)
                .OrderBy(m => m.Order)
                .ProjectTo<MaterialLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new MaterialListVm { Materials =  matherialsQuery };
        }
    }
}
