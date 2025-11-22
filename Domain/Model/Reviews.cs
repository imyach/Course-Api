using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class Reviews : BaseModel
    {
        public Guid IdUser { get; set; }
        public Guid IdCourse { get; set; }
        public int Rait { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
        public Course Course { get; set; }
    }
}
