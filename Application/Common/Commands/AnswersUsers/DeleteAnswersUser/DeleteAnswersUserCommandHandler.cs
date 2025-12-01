using Application.Common.Commands.Matherials.DeleteMatherial;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.AnswersUsers.DeleteAnswersUser
{
    public class DeleteAnswersUserCommandHandler(ICoursesDbContext context) : IRequestHandler<DeleteAnswersUserCommand>
    {
        public async Task<Unit> Handle(DeleteAnswersUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.AnswersUsers.FindAsync([request.Id], cancellationToken);

            if (entity == null || entity.CurrentUserId != request.CurrentUserId)
            {
                throw new NotFoundException(nameof(AnswersUser), request.Id);
            }

            context.AnswersUsers.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
