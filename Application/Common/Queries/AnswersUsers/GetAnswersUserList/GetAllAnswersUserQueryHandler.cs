using Application.Common.Dtos.Answers;
using Application.Common.Dtos.AnswersUsers;
using Application.Common.Exceptions;
using Application.Common.Queries.Answers.GetAnswerList;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static Application.Common.Dtos.AnswersUsers.TestResult.CheckingResponsesDto;

namespace Application.Common.Queries.AnswersUsers.GetAnswersUserList
{
    public class GetAllAnswersUserQueryHandler : IRequestHandler<GetAllAnswersUserQuery, TestHistoryVm>
    {
        private readonly ICoursesDbContext _context;

        public GetAllAnswersUserQueryHandler(ICoursesDbContext context)
        {
            _context = context;
        }

        public async Task<TestHistoryVm> Handle(GetAllAnswersUserQuery request, CancellationToken cancellationToken)
        {
            // 1. Находим TestResult
            var testResult = await _context.TestResults
                .Include(tr => tr.Test)
                    .ThenInclude(t => t.Questions)
                        .ThenInclude(q => q.Answers)
                .Include(tr => tr.AnswersUsers)
                    .ThenInclude(au => au.Answer)
                .FirstOrDefaultAsync(tr => tr.Id == request.TestResultId && tr.UserId == request.CurrentUserId,
                    cancellationToken);

            if (testResult == null)
            {
                return new TestHistoryVm { TestId = request.TestResultId };
            }

            var test = testResult.Test;
            if (test == null)
            {
                return new TestHistoryVm { TestId = request.TestResultId };
            }

            // 2. Группируем ответы пользователя по вопросам
            var userAnswersByQuestion = testResult.AnswersUsers
                .GroupBy(au => au.QuestionId)
                .ToDictionary(g => g.Key, g => g.Select(au => au.AnswerId).ToList());

            // 3. Формируем информацию по каждому вопросу
            var questionsHistory = new List<QuestionHistoryDto>();
            var correctAnswersCount = 0;

            foreach (var question in test.Questions)
            {
                // Получаем все правильные ответы для этого вопроса
                var correctAnswers = question.Answers
                    .Where(a => a.IsCorrect)
                    .ToList();

                // Получаем ответы пользователя на этот вопрос
                var userAnswerIds = userAnswersByQuestion.ContainsKey(question.Id)
                    ? userAnswersByQuestion[question.Id]
                    : new List<Guid>();

                // Находим выбранные пользователем ответы
                var selectedAnswers = question.Answers
                    .Where(a => userAnswerIds.Contains(a.Id))
                    .ToList();

                // Подсчитываем правильность ответа на вопрос
                var correctSelected = selectedAnswers.Count(a => a.IsCorrect);
                var incorrectSelected = selectedAnswers.Count(a => !a.IsCorrect);
                var totalCorrectInQuestion = correctAnswers.Count;

                // Вычисляем процент за вопрос
                double questionScore = 0;
                if (totalCorrectInQuestion > 0)
                {
                    var scoreMultiplier = Math.Max(0, correctSelected - incorrectSelected);
                    questionScore = (scoreMultiplier * 100.0) / totalCorrectInQuestion;
                    questionScore = Math.Round(questionScore, 0);
                }

                var isQuestionCorrect = questionScore >= 99.9;
                if (isQuestionCorrect)
                {
                    correctAnswersCount++;
                }

                // Формируем список ответов для этого вопроса
                var answersHistory = new List<AnswerHistoryDto>();
                foreach (var answer in question.Answers)
                {
                    var isSelected = userAnswerIds.Contains(answer.Id);
                    answersHistory.Add(new AnswerHistoryDto
                    {
                        AnswerId = answer.Id,
                        AnswerText = answer.Text,
                        IsCorrect = answer.IsCorrect,
                        IsSelectedByUser = isSelected,
                        IsCorrectlySelected = answer.IsCorrect && isSelected
                    });
                }

                questionsHistory.Add(new QuestionHistoryDto
                {
                    QuestionId = question.Id,
                    QuestionText = question.Text,
                    IsCorrect = isQuestionCorrect,
                    Score = (int)questionScore,
                    Answers = answersHistory
                });
            }

            // 4. Вычисляем общий результат
            var totalQuestions = test.Questions.Count();
            var overallScore = totalQuestions > 0
                ? (int)Math.Round((correctAnswersCount * 100.0) / totalQuestions, 0)
                : 0;

            return new TestHistoryVm
            {
                TestId = test.Id,
                TestTitle = test.Title,
                TestDescription = test.Description ?? string.Empty,
                TotalQuestions = totalQuestions,
                PassingScore = 80,
                IsTestPassed = testResult.IsPassed,
                BestScore = overallScore,
                CompletedAt = testResult.CompletedAt,
                Questions = questionsHistory
            };
        }
    }
}