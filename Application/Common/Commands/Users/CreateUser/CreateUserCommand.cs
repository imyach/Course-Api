using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Users.CreateUser
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public Guid CurrentUserId { get; set; }
        
        public string NameUser { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string Password { get; set; } = string.Empty;
        public Role? Role { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
