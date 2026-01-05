using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using CourseDesktopClient.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CourseDesktopClient.Models.DtosModel.Entities
{
    public class ReviewDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }
        [JsonPropertyName("courseId")]
        public Guid CourseId { get; set; }
        [JsonPropertyName("rait")]
        public int Rait { get; set; }
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("user")]
        public UserDto User { get; set; } = new();
        [JsonPropertyName("course")]
        public CourseDto Course { get; set; } = new();

    }
}
