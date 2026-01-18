using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Users.UpdateUserForAdmin
{
    public class UpdateUserForAdminCommandHandler(ICoursesDbContext context, IPasswordHasherServise passwordHasher) : IRequestHandler<UpdateUserForAdminCommand, bool>
    {
        public async Task<bool> Handle(UpdateUserForAdminCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.Users.FindAsync([request.Id], cancellationToken) ?? throw new NotFoundException(nameof(User), request.Id);

            var dublicate = await context.Users.AnyAsync(x => (x.Email == request.Email && x.Login == request.Login) && x.Id != request.Id, cancellationToken);

            if (dublicate)
                return false;

            entity.RoleId = request.Role.Id;
            entity.NameUser = request.NameUser;
            entity.Login = request.Login;
            entity.Email = request.Email;
            entity.PhoneNumber = request.PhoneNumber;
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
