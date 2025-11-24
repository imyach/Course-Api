using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class Module : BaseModel
    {
        public Guid IdCourse { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Order { get; set; }

        public Course? Course { get; set; }

        public IEnumerable<Matherial> Matherials { get; set; } = [];

    }
}
