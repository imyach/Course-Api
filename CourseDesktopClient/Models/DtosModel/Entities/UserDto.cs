using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CourseDesktopClient.Models.DtosModel.Entities
{
    public class UserDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("nameUser")]
        public string NameUser { get; set; } = string.Empty;
        [JsonPropertyName("role")]
        public RoleDto? Role { get; set; }
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        [JsonPropertyName("hashPassword")]
        public string HashPassword { get; set; } = string.Empty;
        [JsonPropertyName("login")]
        public string Login { get; set; } = string.Empty;
        [JsonPropertyName("phoneNumber")]
        public string? PhoneNumber { get; set; }
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}
