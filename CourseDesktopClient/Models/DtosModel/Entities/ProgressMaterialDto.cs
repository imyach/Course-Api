using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CourseDesktopClient.Models.DtosModel.Entities
{
    public class ProgressMaterialDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        [JsonPropertyName("startedAt")]
        public DateTime? StartedAt { get; set; }

        [JsonPropertyName("progressModule")]
        public ProgressModuleDto? ProgressModule { get; set; }
        [JsonPropertyName("user")]
        public UserDto? User { get; set; }
        [JsonPropertyName("material")]
        public MaterialDto? Material { get; set; }
    }
}
