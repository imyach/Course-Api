using Application.Common.Commands.ProgressMaterials.DeleteProgressMaterials;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.TestResults.DeleteTestResults
{
    public class DeleteTestResultCommandHandler(ICoursesDbContext context) : IRequestHandler<DeleteTestResultCommand>
    {
        public async Task<Unit> Handle(DeleteTestResultCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entity = await context.TestResults.FindAsync([request.Id], cancellationToken)
                ?? throw new NotFoundException(nameof(TestResults), request.Id);
            if (roleUser.Name == "Admin" || (entity.UserId == currentUser.Id))
            {
                context.TestResults.Remove(entity);
                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            throw new AccessException();
        }
    }
}
