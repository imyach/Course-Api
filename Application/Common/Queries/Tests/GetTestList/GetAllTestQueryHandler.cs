using Application.Common.Dtos.Tests;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Application.Common.Queries.Tests.GetTestList
{
    public class GetAllTestQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetAllTestQuery, TestListVm> 
    {
        public async Task<TestListVm> Handle(GetAllTestQuery request, CancellationToken cancellationToken)
        {
            var testQuery = await context.Tests
                .Include(t => t.Matherial)
                .Where(q => q.MatherialId == request.MaterialId)
                .ProjectTo<TestLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new TestListVm { Tests = testQuery };
        }
    }
}
