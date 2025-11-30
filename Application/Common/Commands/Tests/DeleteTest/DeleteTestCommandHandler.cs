using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Tests.DeleteTest
{
    public class DeleteTestCommandHandler(ICoursesDbContext context) : IRequestHandler<DeleteTestCommand>
    {
        public async Task<Unit> Handle(DeleteTestCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.Tests.FindAsync([request.Id], cancellationToken);
            if (entity == null || entity.CurrentUserId != request.CurrentUserId) 
            {
                throw new NotFoundException(nameof(Test), request.Id);
            }

            context.Tests.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
