using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class ProgressUser : BaseModel
    {
        public Guid CourseId { get; set; }
        public Guid UserId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime StartedAt { get; set; }
        public DateTime? FineshedAt { get; set; }

        public Course? Course { get; set; } 
        public User? User { get; set; }
    }
}
