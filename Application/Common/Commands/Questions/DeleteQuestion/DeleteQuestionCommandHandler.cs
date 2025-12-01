using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Questions.DeleteQuestion
{
    public class DeleteQuestionCommandHandler(ICoursesDbContext context) : IRequestHandler<DeleteQuestionCommand>
    {
        public async Task<Unit> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.Questions.FindAsync([request.Id], cancellationToken);
            if (entity == null || entity.CurrentUserId != request.CurrentUserId)
            {
                throw new NotFoundException(nameof(Question), request.Id);
            }
            context.Questions.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
