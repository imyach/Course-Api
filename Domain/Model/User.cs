using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Domain.Model
{
    public class User : BaseModel
    {
        public string NameUser { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string HashPassword { get; set; } = string.Empty;
        public Guid RoleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }


        public Role? Role { get; set; }
        public RefreshToken? RefreshToken { get; set; }

        public IEnumerable<Course>? Courses { get; set; }
        public IEnumerable<ProgressUser>? ProgressUsers { get; set; }
        public IEnumerable<Review>? Reviews { get; set; }
        public IEnumerable<AnswersUser>? AnswersUsers { get; set; }
    }
}
