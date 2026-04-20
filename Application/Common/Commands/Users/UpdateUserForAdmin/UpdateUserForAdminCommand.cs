using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Users.UpdateUserForAdmin
{
    public class UpdateUserForAdminCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public Guid CurrentUserId { get; set; }
        public string NameUser { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Role Role { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
