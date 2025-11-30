using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class Test : BaseModel
    {
        public Guid? MatherialId { get; set; }
        public Guid? CousreId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public Course? Course { get; set; } 
        public Matherial? Matherial { get; set; }

        public IEnumerable<Question>? Questions { get; set; }
    }
}
