using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Users.UpdateUser
{
    public class UpdateUserCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string NameUser { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string HashPassword { get; set; } = string.Empty;
        public Guid RoleId { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
