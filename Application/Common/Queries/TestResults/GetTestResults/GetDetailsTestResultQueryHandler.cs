using Application.Common.Dtos.ProgressMaterials;
using Application.Common.Dtos.TestResults;
using Application.Common.Exceptions;
using Application.Common.Queries.ProgressMaterials.GetProgressMaterials;
using Application.Interfaces;
using AutoMapper;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.TestResults.GetTestResults
{
    public class GetDetailsTestResultQueryHandler(ICoursesDbContext context, IMapper mapper) : IRequestHandler<GetDetailsTestResultQuery, TestResultLookupDto>
    {
        public async Task<TestResultLookupDto> Handle(GetDetailsTestResultQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entities = await context.TestResults
            .Include(pm => pm.ProgressMaterial)
            .Include(pm => pm.Test)
            .Include(pm => pm.User)
            .ToListAsync(cancellationToken);

            if (roleUser.Name == "Admin" || roleUser.Name == "Couch")
            {

                var entityA = entities.FirstOrDefault(r => r.Id == request.Id)
                    ?? throw new NotFoundException(nameof(TestResult), request.Id);

                return mapper.Map<TestResultLookupDto>(entityA);
            }

            var entity = entities.Where(e => e.UserId == currentUser.Id)
                .FirstOrDefault(r => r.Id == request.Id)
                    ?? throw new NotFoundException(nameof(TestResult), request.Id);

            return mapper.Map<TestResultLookupDto>(entity);
        }
    }
}
