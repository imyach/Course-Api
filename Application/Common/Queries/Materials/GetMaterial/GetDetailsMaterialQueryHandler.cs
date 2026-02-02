using Application.Common.Dtos.Materials;
using Application.Common.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Materials.GetMaterial
{
    public class GetDetailsMaterialQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetDetailsMaterialQuery, MaterialLookupDto>
    {
        public async Task<MaterialLookupDto> Handle(GetDetailsMaterialQuery request, CancellationToken cancellationToken)
        {
            var matherial = await context.Materials
                .Include(m => m.Module)
                    .ThenInclude(m => m.Course)
                    .ThenInclude(c => c.User)
                    .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken)
                    ??throw new NotFoundException(nameof(Material), request.Id);
            
            return mapper.Map<MaterialLookupDto>(matherial);
        }
    }
}
