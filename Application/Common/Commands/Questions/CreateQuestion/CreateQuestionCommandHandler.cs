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

            var entity = await context.Tests
            .Include(u => u.Material)
                .ThenInclude(t => t.Module)
                .ThenInclude(m => m.Course)
            .FirstOrDefaultAsync(u => u.Id == request.TestId, cancellationToken)
            ?? throw new NotFoundException(nameof(Question), request.TestId);

            if (roleUser.Name == "Admin" || (roleUser.Name == "Couch" && entity.Material.Module.Course.UserId == currentUser.Id))
            {
                var question = new Question
                {
                    Id = Guid.NewGuid(),
                    TestId = request.TestId,
                    Text = request.Text,
                };
                
                if (entity.Material.Module.Course.Status == "Published")
                    entity.Material.Module.Course.UpdateAt = DateTime.UtcNow;
                await context.Questions.AddAsync(question, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return question.Id;
            }
            throw new AccessException();
        }
    }
}
