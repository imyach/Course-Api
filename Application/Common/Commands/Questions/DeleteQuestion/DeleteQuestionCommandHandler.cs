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
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entity = await context.Questions
            .Include(u => u.Test)
                .ThenInclude(t=>t.Matherial)
                .ThenInclude(m => m.Module)
                .ThenInclude(m => m.Course)
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Question), request.Id);

            if (roleUser.RoleName == "Admin" || (roleUser.RoleName == "Couch" && currentUser.Id == entity.Test.Matherial.Module.Course.UserId))
            {
                if(entity.Test.Matherial.Module.Course.Status == "Published")
                    entity.Test.Matherial.Module.Course.UpdateAt = DateTime.UtcNow;
                context.Questions.Remove(entity);
                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            throw new AccessException();
        }
    }
}
