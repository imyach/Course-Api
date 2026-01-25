using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CourseDesktopClient.Models.DtosModel.Entities
{
    public class ProgerssInfoDto
    {
        [JsonPropertyName("completedCourse")]
        public int CompletedCourse { get; set; }

        [JsonPropertyName("courseInPassage")]
        public int CourseInPassage { get; set; }
    }
}
