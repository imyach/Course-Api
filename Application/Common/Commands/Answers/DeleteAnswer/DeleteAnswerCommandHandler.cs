using Application.Common.Commands.Matherials.DeleteMatherial;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Answers.DeleteAnswer
{
    public class DeleteAnswerCommandHandler(ICoursesDbContext context) : IRequestHandler<DeleteAnswerCommand>
    {
        public async Task<Unit> Handle(DeleteAnswerCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.Answers.FindAsync([request.Id], cancellationToken);

            if (entity == null || entity.CurrentUserId != request.CurrentUserId)
            {
                throw new NotFoundException(nameof(Answer), request.Id);
            }

            context.Answers.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
