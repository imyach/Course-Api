using Application.Common.Commands.Matherials.UpdateMatherial;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Answers.UpdateAnswer
{
    public class UpdateAnswerCommandHandler(ICoursesDbContext context) : IRequestHandler<UpdateAnswerCommand>
    {
        public async Task<Unit> Handle(UpdateAnswerCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.Answers.FindAsync([request.Id], cancellationToken);
            if (entity == null || entity.CurrentUserId != request.CurrentUserId)
            {
                throw new NotFoundException(nameof(Answer), request.Id);
            }
            entity.Text = request.Text;
            entity.IsCorrect = request.IsCorrect;

            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
