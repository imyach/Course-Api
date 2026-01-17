using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows.Input;

namespace CourseDesktopClient.Models.DtosModel.Entities
{
    public class ProgressUserDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        [JsonPropertyName("startedAt")]
        public DateTime StartedAt { get; set; }
        [JsonPropertyName("fineshedAt")]
        public DateTime? FineshedAt { get; set; }

        [JsonPropertyName("course")]
        public CourseDto? Course { get; set; }
        [JsonPropertyName("user")]
        public UserDto? User { get; set; }
    }
}
