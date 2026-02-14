using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.Models.DtosModel.Entities.RequestDto
{
    public class UserRequestDto
    {
        public string NameUser { get; set; } = string.Empty;
        public RoleDto Role { get; set; } = new();
        public string? Email { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }
}
