using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Users.DeteleUser
{
    public class DeleteUserCommandHandler(ICoursesDbContext context) : IRequestHandler<DeleteUserCommand>
    {
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);
            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entity = await context.Users.FindAsync([request.Id], cancellationToken);
            if (entity == null || (currentUser.Id != entity.Id && roleUser.RoleName != "Admin"))
            {
                throw new NotFoundException(nameof(User), request.Id);
                throw new Exception("User have not role Admin");
            }
            context.Users.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
