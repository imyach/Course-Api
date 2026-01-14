using Application.Common.Dtos.Auth;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Auth.Refresh
{
    public class RefreshTokenCommandHandler(ICoursesDbContext context, IJwtTokenServise tokenService) : IRequestHandler<RefreshTokenCommand, TokensDto?>
    {
        public async Task<TokensDto?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await context.RefreshTokens
                .FirstOrDefaultAsync(x => x.ResreshToken == request.RefreshToken, cancellationToken);

            if (token is null)
                return null; 

            if (token.ExpiresIn <= DateTime.UtcNow)
                return null; 

            var user = await context.Users.FindAsync([token.UserId], cancellationToken);
            if (user is null)
                return null;

            context.RefreshTokens.Remove(token);
            await context.SaveChangesAsync(cancellationToken);

            return await tokenService.GenerateTokens(user);
        }
    }
}
