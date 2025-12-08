using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CourseDesktopClient.Models.DtosModel.Entities
{
    public class RoleDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}
