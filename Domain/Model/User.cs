using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Domain.Model
{
    public class User : BaseModel
    {
        public string NameUser { get; set; }
        public string Login { get; set; }
        public string? Email { get; set; }
        public string HashPassword { get; set; }
        public Guid RoleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? PhoneNumber { get; set; }

        public Role Role { get; set; }

        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<ProgressUser> ProgressUsers { get; set; }
        public IEnumerable<Reviews> Reviews { get; set; }
        public IEnumerable<AnswersUser> AnswersUsers { get; set; }
    }
}
