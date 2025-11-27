using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class Course : BaseModel
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public decimal Rait { get; set; } = 0;
        public Guid  UserId { get; set; }

        public User? User { get; set; }

        public IEnumerable<ProgressUser> ProgressUsers { get; set; } = [];
        public IEnumerable<Reviews> Reviews { get; set; } = [];
        public IEnumerable<Module> Modules { get; set; } = [];
        public IEnumerable<Test> Tests { get; set; } = [];

    }
}
