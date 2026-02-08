using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CourseDesktopClient.Models.DtosModel.Entities
{
    public class AnswersUserDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("user")]
        public UserDto? User { get; set; }
        [JsonPropertyName("answer")]
        public AnswerDto? Answer { get; set; }
        [JsonPropertyName("question")]
        public QuestionDto? Question { get; set; }
        [JsonPropertyName("testResult")]
        public TestResultDto? TestResult { get; set; }
    }
}
