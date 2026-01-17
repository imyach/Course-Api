using CourseDesktopClient.Models.DtosModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CourseDesktopClient.Models.DtosModel.Auth.RequestDto
{
    public class UpdateUserRequestDto
    {
        public Guid Id { get; set; }
        public string NameUser { get; set; } = string.Empty;
        public RoleDto Role { get; set; } = new();
        public string? Email { get; set; }
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }
}
