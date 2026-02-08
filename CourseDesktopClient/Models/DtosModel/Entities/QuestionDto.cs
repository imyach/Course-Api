using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CourseDesktopClient.Models.DtosModel.Entities
{
    public class QuestionDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        [JsonPropertyName("test")]
        public TestDto? Test { get; set; }
        public Guid TestId { get; set; }
        
        [JsonPropertyName("answers")]
        public IList<AnswerDto>? Answers { get; set; }
    }
}
