using Application.Common.Dtos.Matherials;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Matherials.GetMatherialList
{
    public class GetAllMatherialQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetAllMatherialQuery, MatherialListVm>
    {
        public async Task<MatherialListVm> Handle(GetAllMatherialQuery request, CancellationToken cancellationToken)
        {
            var matherialsQuery = await context.Matherials
                .Include(m => m.Module)
                .Where(m=>m.ModuleId == request.ModuleId)
                .ProjectTo<MatherialLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new MatherialListVm { Matherials =  matherialsQuery };
        }
    }
}
