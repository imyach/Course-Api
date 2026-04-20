using Application.Common.Commands.Materials.CreateMaterial;
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

            var entity = await context.Questions
           .Include(a => a.Test)
               .ThenInclude(q => q.Material)
               .ThenInclude(t => t.Module)
               .ThenInclude(m => m.Course)
           .FirstOrDefaultAsync(u => u.Id == request.QuestionId, cancellationToken)
           ?? throw new NotFoundException(nameof(Answer), request.QuestionId);

            if (roleUser.Name == "Admin" || (roleUser.Name == "Couch" && entity.Test.Material.Module.Course.UserId == currentUser.Id))
            {
                var answer = new Answer
                {
                    Id = Guid.NewGuid(),
                    QuestionId = request.QuestionId,
                    Text = request.Text,
                    IsCorrect = request.IsCorrect,
                };
                
                if (entity.Test.Material.Module.Course.Status == "Published")
                    entity.Test.Material.Module.Course.UpdateAt = DateTime.UtcNow;

                await context.Answers.AddAsync(answer, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return answer.Id;
            }
            throw new AccessException();
        }
    }
}
