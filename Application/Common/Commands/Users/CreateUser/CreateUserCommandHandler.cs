using Application.Common.Commands.Users.CreateUser;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Users.CreateUser
{
    public class CreateUserCommandHandler(ICoursesDbContext context, IPasswordHasherServise passwordHasher) : IRequestHandler<CreateUserCommand, Guid>
    {
        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await context.Users.FindAsync([request.CurrentUserId], cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.CurrentUserId);
            var roleUser = await context.Roles.FirstOrDefaultAsync(r => r.Id == currentUser.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role), currentUser.RoleId);

            if (roleUser.Name == "Admin")
            {
                var dublicate = await context.Users.AnyAsync(x => x.Email == request.Email && x.Login == request.Login, cancellationToken);

                if (dublicate)
                    return Guid.Empty;

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    RoleId = request.Role.Id,
                    NameUser = request.NameUser,
                    Login = request.Login,
                    Email = request.Email,
                    HashPassword = passwordHasher.HashPasword(request.Password),
                    CreatedAt = DateTime.UtcNow,
                    PhoneNumber = request.PhoneNumber,
                    IsActive = true
                };
                await context.Users.AddAsync(user, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return user.Id;
            }
            throw new AccessException();
        }
    }
}
