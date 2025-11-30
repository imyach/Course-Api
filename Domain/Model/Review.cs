using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class Review : BaseModel
    {
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public int Rait { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public User? User { get; set; }
        public Course? Course { get; set; }
    }
}
