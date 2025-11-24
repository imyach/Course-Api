using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Users.CreateUser
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string NameUser { get; set; }
        public string Login { get; set; }
        public string? Email { get; set; }
        public string HashPassword { get; set; }
        public Guid RoleId { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
