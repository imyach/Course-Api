using Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Domain.Model
{
    public class TestResult: BaseModel
    {
        public Guid UserId { get; set; }
        public Guid TestId { get; set; }
        public Guid ProgressMaterialId { get; set; }
        public int Score { get; set; }
        public bool IsPassed { get; set; }
        public DateTime? CompletedAt { get; set; }

        public User? User { get; set; }
        public Test? Test { get; set; }
        public ProgressMaterial? ProgressMaterial { get; set; }

        public IEnumerable<AnswersUser>? AnswersUsers { get; set; }
    }
}
