using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Matherials.UpdateMatherial
{
    public class UpdateMatherialCommandHandler(ICoursesDbContext context) : IRequestHandler<UpdateMatherialCommand>
    {
        public async Task<Unit> Handle(UpdateMatherialCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.Matherials.FindAsync([request.Id], cancellationToken);
            if (entity == null || entity.CurrentUserId != request.CurrentUserId)
            {
                throw new NotFoundException(nameof(Matherial), request.Id);
            }
            entity.Description = request.Description;
            entity.Order = request.Order;
            entity.Title = request.Title;   
            entity.IdModule = request.IdModule;

            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
