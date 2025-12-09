using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CourseWebApi.Servises
{
    public class JwtTokenServise(ICoursesDbContext context, IConfiguration configuration) : IJwtTokenServise
    {
        string SECRET_KEY = configuration["SECRET_KEY"];
        public TimeSpan ExpiryDuration = new(30, 0, 0);
        public async Task<string> GenerateJwtToken(User user) 
        {

            var role = await context.Roles.FindAsync([user.RoleId]);
            string roleNameClaim = role?.RoleName.ToString() ?? string.Empty;

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
    }
}
