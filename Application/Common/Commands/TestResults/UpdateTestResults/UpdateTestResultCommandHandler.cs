using Application.Common.Commands.ProgressMaterials.UpdateProgressMaterials;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.TestResults.UpdateTestResults
{
    public class UpdateTestResultCommandHandler(ICoursesDbContext context) : IRequestHandler<UpdateTestResultCommand>
    {
        public async Task<Unit> Handle(UpdateTestResultCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.TestResults.FindAsync([request.Id], cancellationToken)
               ?? throw new NotFoundException(nameof(TestResults), request.Id);

            if (entity.UserId != request.CurrentUserId)
                throw new AccessException();

            if (request.Score != 0)
                entity.Score = request.Score;
            if (request.ComplitedAt is not null)
                entity.CompletedAt = request.ComplitedAt;
            if (request.IsPassed is not false)
                entity.IsPassed = request.IsPassed;

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
