using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class ProgressUser : BaseModel
    {
        public Guid IdCourse { get; set; }
        public Guid IdUser { get; set; }
        public string Status { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? FineshedAt { get; set; }

        public Course Course { get; set; }
        public User User { get; set; }
    }
}
