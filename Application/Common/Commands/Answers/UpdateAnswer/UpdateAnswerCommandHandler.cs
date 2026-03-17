using Application.Common.Commands.Materials.UpdateMaterial;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Answers.UpdateAnswer
{
    public class UpdateAnswerCommandHandler(ICoursesDbContext context) : IRequestHandler<UpdateAnswerCommand>
    {
        public async Task<Unit> Handle(UpdateAnswerCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entity = await context.Answers
                .Include(a => a.Question)
                    .ThenInclude(q => q.Test)
                    .ThenInclude(t => t.Material)
                    .ThenInclude(m => m.Module)
                    .ThenInclude(m => m.Course)
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Answer), request.Id);

            if (roleUser.RoleName == "Admin" || (roleUser.RoleName == "Couch" && currentUser.Id == entity.Question.Test.Material.Module.Course.UserId))
            {

                if (!string.IsNullOrEmpty(request.Text))
                    entity.Text = request.Text;
                if (request.IsCorrect != null)
                entity.IsCorrect = request.IsCorrect;

                if (entity.Question.Test.Material.Module.Course.Status == "Published")
                    entity.Question.Test.Material.Module.Course.UpdateAt = DateTime.UtcNow;

                await context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
            throw new AccessException();

        }
    }
}
