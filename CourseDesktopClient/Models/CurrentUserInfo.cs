using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.Models
{
    public class CurrentUserInfo
    {
        public string Id { get; set; } = string.Empty;
        public string NameUser { get; set; } = string.Empty;
        public string RoleUser { get; set; } = string.Empty;
        public string EmailUser { get; set; } = string.Empty;
    }
}
