using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.Models.DtosModel.Entities.RequestDto
{
    public class ProgressUserRequestDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime FinishedAt { get; set; }
        public Guid CourseId { get; set; }
    }
}
