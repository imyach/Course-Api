using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Questions.UpdateQuestion
{
    public class UpdateQuestionCommandHandler(ICoursesDbContext context) : IRequestHandler<UpdateQuestionCommand>
    {
        public async Task<Unit> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.Questions.FindAsync([request.Id], cancellationToken);
            if (entity == null || entity.CurrentUserId != request.CurrentUserId)
            {
                throw new NotFoundException(nameof(Question), request.Id);
            }

            entity.Text = request.Text;
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
