using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Users.DeteleUser
{
    public class DeleteUserCommandHandler(ICoursesDbContext context) : IRequestHandler<DeleteUserCommand>
    {
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.Users.FindAsync([request.Id], cancellationToken);
            if (entity == null || request.Id != entity.UserId) 
            {
                throw new NotFoundException(nameof(User), request.Id);
            }

            context.Users.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
