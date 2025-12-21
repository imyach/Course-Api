using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Auth.Logout
{
    public class LogoutUserCommandHandler(ICoursesDbContext context, IJwtTokenServise tokenService) : IRequestHandler<LogoutUserCommand, Unit?>
    {
        public async Task<Unit?> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            var user = await context.Users.FindAsync([request.CurrentUserId], cancellationToken);
            if (user is null)
                return null;

            await tokenService.DeleteResreshToken(user, cancellationToken);

            return Unit.Value;
        }
    }
}
