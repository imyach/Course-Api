using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Tests.UpdateTest
{
    public class UpdateTestCommandHandler(ICoursesDbContext context) : IRequestHandler<UpdateTestCommand>
    {
        public async Task<Unit> Handle(UpdateTestCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.Tests.FindAsync([request.Id], cancellationToken);
            if (entity == null || entity.CurrentUserId != request.CurrentUserId)
            {
                throw new NotFoundException(nameof(Test), request.Id);
            }

            entity.Title = request.Title;
            entity.Description = request.Description;
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
