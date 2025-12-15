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
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);
            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            var entity = await context.Users.FindAsync([request.Id], cancellationToken) ?? throw new NotFoundException(nameof(User), currentUser.Id);

            var dulicate = await context.Users.FirstOrDefaultAsync(x => (x.Login == request.Login && x.HashPassword == passwordHasher.HashPasword(request.Password))
                    || (x.Email == request.Email && x.HashPassword == passwordHasher.HashPasword(request.Password)), cancellationToken);

            if (dulicate is not null)
            {
                return false;
            }

            entity.RoleId = request.RoleId;
            entity.NameUser = request.NameUser;
            entity.Login = request.Login;
            entity.Email = request.Email;
            entity.HashPassword = passwordHasher.HashPasword(request.Password);
            entity.PhoneNumber = request.PhoneNumber;
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
