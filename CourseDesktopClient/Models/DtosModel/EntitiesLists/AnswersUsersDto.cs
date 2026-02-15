using CourseDesktopClient.Models.DtosModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CourseDesktopClient.Models.DtosModel.EntitiesLists
{
    public class TestHistoryVm
    {
        [JsonPropertyName("testId")]
        public Guid TestId { get; set; }

        [JsonPropertyName("testTitle")]
        public string TestTitle { get; set; } = string.Empty;

        [JsonPropertyName("testDescription")]
        public string TestDescription { get; set; } = string.Empty;

        [JsonPropertyName("totalQuestions")]
        public int TotalQuestions { get; set; }

        [JsonPropertyName("passingScore")]
        public int PassingScore { get; set; }

        [JsonPropertyName("isTestPassed")]
        public bool IsTestPassed { get; set; }

        [JsonPropertyName("bestScore")]
        public int BestScore { get; set; }

        [JsonPropertyName("completedAt")]
        public DateTime? CompletedAt { get; set; }

        [JsonPropertyName("questions")]
        public List<QuestionHistoryDto> Questions { get; set; } = new();
    }

    public class QuestionHistoryDto
    {
        [JsonPropertyName("questionId")]
        public Guid QuestionId { get; set; }

        [JsonPropertyName("questionText")]
        public string QuestionText { get; set; } = string.Empty;

        [JsonPropertyName("isCorrect")]
        public bool IsCorrect { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("answers")]
        public List<AnswerHistoryDto> Answers { get; set; } = new();
    }

    public class AnswerHistoryDto
    {
        [JsonPropertyName("answerId")]
        public Guid AnswerId { get; set; }

        [JsonPropertyName("answerText")]
        public string AnswerText { get; set; } = string.Empty;

        [JsonPropertyName("isCorrect")]
        public bool IsCorrect { get; set; }

        [JsonPropertyName("isSelectedByUser")]
        public bool IsSelectedByUser { get; set; }

        [JsonPropertyName("isCorrectlySelected")]
        public bool IsCorrectlySelected { get; set; }
    }
}
