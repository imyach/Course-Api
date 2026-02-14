using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.Models.DtosModel.Entities
{
    public class QuestionResultDto
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public int Score { get; set; }
        public bool IsFullyCorrect { get; set; }
    }
}
