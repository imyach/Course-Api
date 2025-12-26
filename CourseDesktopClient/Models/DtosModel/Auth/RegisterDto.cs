using CourseDesktopClient.Models.DtosModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CourseDesktopClient.Models.DtosModel.Auth
{
    public class RegisterDto
    {
        public string NameUser { get; set; } = string.Empty;
        public RoleDto Role { get; set; } = new();
        public string? Email { get; set; }
        public string HashPassword { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }
}
