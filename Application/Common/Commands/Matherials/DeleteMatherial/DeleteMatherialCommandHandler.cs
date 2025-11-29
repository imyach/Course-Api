using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Matherials.DeleteMatherial
{
    public class DeleteMatherialCommandHandler(ICoursesDbContext context) : IRequestHandler<DeleteMatherialCommand>
    {
        public async Task<Unit> Handle(DeleteMatherialCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.Matherials.FindAsync([request.Id], cancellationToken);

            if (entity == null || entity.CurrentUserId != request.CurrentUserId)
            {
                throw new NotFoundException(nameof(Matherial), request.Id);
            }

            context.Matherials.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
