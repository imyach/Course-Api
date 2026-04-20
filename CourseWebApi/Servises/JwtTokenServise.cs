using Application.Common.Dtos.Auth;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CourseWebApi.Servises
{
    public class JwtTokenServise(ICoursesDbContext context, IConfiguration configuration) : IJwtTokenServise
    {
        string SECRET_KEY = configuration["SECRET_KEY"];
        public TimeSpan ExpiryDuration = new(0, 15, 0);
        private async Task<string> GenerateJwtToken(User user, CancellationToken cancellationToken = default)
        {

            var role = await context.Roles.FindAsync([user.RoleId], cancellationToken);
            string roleNameClaim = role?.Name.ToString() ?? string.Empty;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SECRET_KEY);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([
                    new Claim(ClaimTypes.Name, user.NameUser),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, roleNameClaim),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                ]),
                Expires = DateTime.UtcNow.Add(ExpiryDuration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Audience = "CourseWebApi"
            };

            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }

        public async Task<Guid> GenerateRefreshToken(User user, CancellationToken cancellationToken = default)
        {

            var newToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                ResreshToken = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                ExpiresIn = DateTime.UtcNow.AddDays(30)
            };

            await context.RefreshTokens.AddAsync(newToken, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return newToken.ResreshToken;

        }


        public async Task<TokensDto> GenerateTokens(User user)
        {
            await DeleteResreshToken(user);

            var refreshToken = await GenerateRefreshToken(user);
            var accessToken = await GenerateJwtToken(user);

            return new TokensDto { AccessToken = accessToken, RefreshToken = refreshToken };

        }
        public async Task DeleteResreshToken(User user, CancellationToken cancellationToken = default)
        {
            var tokens = await context.RefreshTokens
                .Where(rt => rt.UserId == user.Id)
                .ToListAsync(cancellationToken);
            if (tokens is not null)
            {
                context.RefreshTokens.RemoveRange(tokens);
                await context.SaveChangesAsync(cancellationToken);
            }
        }

    }
}
