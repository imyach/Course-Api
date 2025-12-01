using Application.Common.Commands.Matherials.CreateMatherial;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Answers.CreateAnswer
{
    public class CreateAnswerCommandHandler(ICoursesDbContext context) : IRequestHandler<CreateAnswerCommand, Guid>
    {
        public async Task<Guid> Handle(CreateAnswerCommand request, CancellationToken cancellationToken)
        {
            var answer = new Answer
            {
                Id = Guid.NewGuid(),
                QuestionId = request.QuestionId,
                Text = request.Text,
                IsCorrect = request.IsCorrect,
            };

            await context.Answers.AddAsync(answer, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return answer.Id;
        }
    }
}
