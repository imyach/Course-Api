using Application.Common.Commands.ProgressModules.CreateProgressModules;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.TestResults.CreateTestResults
{
    public class CreateTestResultCommandHandler(ICoursesDbContext context) : IRequestHandler<CreateTestResultCommand, Guid>
    {
        public async Task<Guid> Handle(CreateTestResultCommand request, CancellationToken cancellationToken)
        {
            var testResult = new TestResult
            {
                Id = Guid.NewGuid(),
                TestId = request.TestId,
                ProgressMaterialId = request.ProgressMaterialId,
                UserId = request.CurrentUserId,
                Score = 0,
                IsPassed = false,
                ComplitedAt = null
            };


            await context.TestResults.AddAsync(testResult, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return testResult.Id;
        }
    }
}
