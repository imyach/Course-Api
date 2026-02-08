using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Domain.Model
{
    public class ProgressMaterial : BaseModel
    {
        public Guid UserId { get; set; }
        public Guid MaterialId { get; set; }
        public Guid ProgressModuleId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? StartedAt { get; set; }
        public int Order { get; set; }
        


        public User? User { get; set; }
        public Material? Material { get; set; }
        public ProgressModule? ProgressModule { get; set; }

        public IEnumerable<TestResult>? TestResult { get; set; }
    }
}
