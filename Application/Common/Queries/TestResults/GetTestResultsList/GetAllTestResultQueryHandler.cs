using Application.Common.Dtos.ProgressModules;
using Application.Common.Dtos.TestResults;
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

namespace Application.Common.Queries.TestResults.GetTestResultsList
{
    public class GetAllTestResultQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetAllTestResultQuery, TestResultListVm>
    {
        public async Task<TestResultListVm> Handle(GetAllTestResultQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var query = await context.TestResults
                .Where(m => m.UserId == request.CurrentUserId && m.ProgressMaterialId == request.ProgressMaterialId)
                .Include(pm => pm.Test)
                .Include(m => m.ProgressMaterial)
                .Include(m => m.User)
                .ProjectTo<TestResultLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new TestResultListVm { TestResults = query };
        }
    }
}
