using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.AnswersUsers.TestResult
{
    public class CheckingResponsesDto
    {
        public class CompleteTestResponseDto
        {
            public bool IsPassed { get; set; }
            public int Score { get; set; }
            public int TotalQuestions { get; set; }
            public int CorrectAnswers { get; set; }
            public Guid TestId { get; set; }
            public string TestTitle { get; set; } = string.Empty;
            public List<QuestionResultDto> QuestionResults { get; set; } = new();
        }

        public class QuestionResultDto
        {
            public Guid QuestionId { get; set; }
            public string QuestionText { get; set; } = string.Empty;
            public int Score { get; set; }
            public bool IsFullyCorrect { get; set; }
            public List<AnswerResultDto> CorrectAnswers { get; set; } = new();
            public List<AnswerResultDto> IncorrectAnswersSelected { get; set; } = new();
            public List<AnswerResultDto> CorrectAnswersMissed { get; set; } = new();
        }

        public class AnswerResultDto
        {
            public Guid AnswerId { get; set; }
            public string AnswerText { get; set; } = string.Empty;
            public bool IsSelectedByUser { get; set; }
        }
        public class CompleteTestRequestDto
        {
            public Guid TestResultId { get; set; }
            public List<SelectedAnswerDto> SelectedAnswers { get; set; } = new();
            public DateTime CompletedAt { get; set; }
        }

        public class SelectedAnswerDto
        {
            public Guid QuestionId { get; set; }
            public Guid AnswerId { get; set; }
        }

        public class TestHistoryVm
        {
            public Guid TestId { get; set; }
            public string TestTitle { get; set; } = string.Empty;
            public string TestDescription { get; set; } = string.Empty;
            public int TotalQuestions { get; set; }
            public int PassingScore { get; set; }
            public bool IsTestPassed { get; set; }
            public int BestScore { get; set; }
            public DateTime? CompletedAt { get; set; }
            public List<QuestionHistoryDto> Questions { get; set; } = new();
        }

        public class QuestionHistoryDto
        {
            public Guid QuestionId { get; set; }
            public string QuestionText { get; set; } = string.Empty;
            public bool IsCorrect { get; set; } 
            public int Score { get; set; } 
            public List<AnswerHistoryDto> Answers { get; set; } = new();
        }

        public class AnswerHistoryDto
        {
            public Guid AnswerId { get; set; }
            public string AnswerText { get; set; } = string.Empty;
            public bool IsCorrect { get; set; } 
            public bool IsSelectedByUser { get; set; } 
            public bool IsCorrectlySelected { get; set; }
        }
    }
}
