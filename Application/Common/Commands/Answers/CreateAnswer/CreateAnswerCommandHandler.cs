using Application.Common.Commands.Matherials.CreateMatherial;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Answers.CreateAnswer
{
    public class CreateAnswerCommandHandler(ICoursesDbContext context) : IRequestHandler<CreateAnswerCommand, Guid>
    {
        public async Task<Guid> Handle(CreateAnswerCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var question = await context.Questions.FirstOrDefaultAsync(r => r.Id == request.QuestionId, cancellationToken)
                ?? throw new NotFoundException(nameof(Course), request.QuestionId);

            if (roleUser.RoleName == "Admin" || (roleUser.RoleName == "Couch" && question.Test.Course.UserId == currentUser.Id))
            {
                var answer = new Answer
                {
                    Id = Guid.NewGuid(),
                    QuestionId = request.QuestionId,
                    Text = request.Text,
                    IsCorrect = request.IsCorrect,
                };

                question.Test.Course.UpdateAt = DateTime.Now;
                await context.Answers.AddAsync(answer, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return answer.Id;
            }
            throw new AccessException();
        }
    }
}
