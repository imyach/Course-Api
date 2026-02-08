using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CourseDesktopClient.Models.DtosModel.Entities
{
    public class ProgressModuleDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        [JsonPropertyName("startedAt")]
        public DateTime? StartedAt { get; set; }

        [JsonPropertyName("progressUser")]
        public ProgressUserDto? ProgressUser { get; set; }
        [JsonPropertyName("user")]
        public UserDto? User { get; set; }
        [JsonPropertyName("module")]
        public ModuleDto? Module { get; set; }
    }
}
