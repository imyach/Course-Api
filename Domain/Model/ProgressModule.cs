using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class ProgressModule : BaseModel
    {
        public Guid UserId { get; set; }
        public Guid ModuleId { get; set; }
        public Guid ProgressUserId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? StartedAt { get; set; }

        public User? User { get; set; }
        public Module? Module { get; set; }
        public ProgressUser? ProgressUser { get; set; }

        public IEnumerable<ProgressMaterial>? ProgressMaterial { get; set; }
    }
}
