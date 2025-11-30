using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class Matherial : BaseModel
    {
        public Guid ModuleId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Order { get; set; }

        public Module? Module { get; set; }

        public IEnumerable<Test>? Tests { get; set; }
    }
}
