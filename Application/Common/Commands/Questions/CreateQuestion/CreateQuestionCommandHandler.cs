using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Questions.CreateQuestion
{
    public class CreateQuestionCommandHandler(ICoursesDbContext context) : IRequestHandler<CreateQuestionCommand, Guid>
    {
        public async Task<Guid> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = new Question
            {
                Id = Guid.NewGuid(),
                TestId = request.TestId,
                Text = request.Text,
            };

            await context.Questions.AddAsync(question, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return question.Id;
        }
    }
}
