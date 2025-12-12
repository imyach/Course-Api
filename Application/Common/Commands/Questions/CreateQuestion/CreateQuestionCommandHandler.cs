using Application.Common.Exceptions;
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
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var test = await context.Tests.FirstOrDefaultAsync(r => r.Id == request.TestId, cancellationToken)
                ?? throw new NotFoundException(nameof(Test), request.TestId);

            if (roleUser.RoleName == "Admin" || (roleUser.RoleName == "Couch" && test.Course.UserId == currentUser.Id))
            {
                var question = new Question
                {
                    Id = Guid.NewGuid(),
                    TestId = request.TestId,
                    Text = request.Text,
                };

                test.Course.UpdateAt = DateTime.Now;
                await context.Questions.AddAsync(question, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return question.Id;
            }
            throw new AccessException();
        }
    }
}
