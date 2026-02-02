using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class Module : BaseModel
    {
        public Guid CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Order { get; set; } = 0;

        public Course? Course { get; set; }

        public IEnumerable<Material>? Materials { get; set; }
        public IEnumerable<ProgressModule>? ProgressModules { get; set; }
    }
}
