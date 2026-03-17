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


            if (request.Role != null)
             entity.RoleId = request.Role.Id;
            if (!string.IsNullOrEmpty(request.NameUser))
                entity.NameUser = request.NameUser;
            if (!string.IsNullOrEmpty(request.Login))
                entity.Login = request.Login;
            if (!string.IsNullOrEmpty(request.Email))
                entity.Email = request.Email;
            if (!string.IsNullOrEmpty(request.PhoneNumber))
                entity.PhoneNumber = request.PhoneNumber;
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
