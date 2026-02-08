using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class Test : BaseModel
    {
        public Guid? MaterialId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Order { get; set; } = 0;

        public Material? Material { get; set; }

        public IEnumerable<Question>? Questions { get; set; }
        public IEnumerable<TestResult>? TestResult { get; set; }
    }
}
