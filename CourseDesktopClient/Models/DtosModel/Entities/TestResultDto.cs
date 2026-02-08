using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CourseDesktopClient.Models.DtosModel.Entities
{
    public class TestResultDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("score")]
        public int Score { get; set; }
        [JsonPropertyName("isPassed")]
        public bool IsPassed { get; set; }
        [JsonPropertyName("completedAt")]
        public DateTime CompletedAt { get; set; }

        [JsonPropertyName("progressMaterial")]
        public ProgressMaterialDto? ProgressMaterial { get; set; }
        [JsonPropertyName("user")]
        public UserDto? User { get; set; }
        [JsonPropertyName("test")]
        public TestDto? Test { get; set; }
    }
}
