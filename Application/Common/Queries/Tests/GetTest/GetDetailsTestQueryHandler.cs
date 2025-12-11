using Application.Common.Dtos.Tests;
using Application.Common.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Tests.GetTest
{
    public class GetDetailsTestQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetDetailsTestQuery, TestLookupDto>
    {
        public async Task<TestLookupDto> Handle(GetDetailsTestQuery request, CancellationToken cancellationToken)
        {
            var entity = await context.Tests
                .Include(t=> t.Matherial)
                    .ThenInclude(m=> m.Module)
                    .ThenInclude(m=>m.Course)
                    .ThenInclude(c=>c.User)
                    .ThenInclude(u=>u.Role)
                .Include(t=> t.Course)
                    .ThenInclude(c => c.User)
                    .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(t=>t.Id == request.Id, cancellationToken)
                ??throw new NotFoundException(nameof(Test),request.Id);

            return mapper.Map<TestLookupDto>(entity);
        }
    }
}
