using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Users.UpdateUser
{
    public class UpdateUserCommandHandler(ICoursesDbContext context) : IRequestHandler<UpdateUserCommand>
    {
        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.Users.FindAsync([request.Id], cancellationToken);
            if (entity == null || request.Id != entity.UserId) 
            {
                throw new NotFoundException(nameof(User), request.Id);
            }
             
            entity.RoleId = request.RoleId;
            entity.NameUser = request.NameUser;
            entity.Login = request.Login;
            entity.Email = request.Email;
            entity.HashPassword = request.HashPassword;
            entity.PhoneNumber = request.PhoneNumber;
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
