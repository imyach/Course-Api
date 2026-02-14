using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CourseDesktopClient.Models.DtosModel.Entities
{
    public class CompleteTestResponseDto
    {
        [JsonPropertyName("isPassed")]
        public bool IsPassed { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("totalQuestions")]
        public int TotalQuestions { get; set; }

        [JsonPropertyName("correctAnswers")]
        public int CorrectAnswers { get; set; }

        [JsonPropertyName("testId")]
        public Guid TestId { get; set; }

        [JsonPropertyName("testTitle")]
        public string TestTitle { get; set; } = string.Empty;

        [JsonPropertyName("questionResults")]
        public List<QuestionResultDto> QuestionResults { get; set; } = new();
    }
}
