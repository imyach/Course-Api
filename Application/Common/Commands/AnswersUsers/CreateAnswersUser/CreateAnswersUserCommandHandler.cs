using Application.Common.Commands.Materials.CreateMaterial;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using static Application.Common.Dtos.AnswersUsers.TestResult.CheckingResponsesDto;

namespace Application.Common.Commands.AnswersUsers.CreateAnswersUser
{
    public class CreateAnswersUserCommandHandler : IRequestHandler<CreateAnswersUserCommand, CompleteTestResponseDto>
    {
        private readonly ICoursesDbContext _context;

        public CreateAnswersUserCommandHandler(
            ICoursesDbContext context)
        {
            _context = context;
        }

        public async Task<CompleteTestResponseDto> Handle(CreateAnswersUserCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

                var testResult = await _context.TestResults
                    .Include(tr => tr.Test)
                    .Include(tr => tr.ProgressMaterial)
                    .FirstOrDefaultAsync(tr => tr.Id == request.TestResultId, cancellationToken);

                if (testResult == null)
                {
                    throw new Exception($"TestResult с ID {request.TestResultId} не найден");
                }

                if (testResult.UserId != request.CurrentUserId)
                {
                    throw new UnauthorizedAccessException("Нет доступа к этому тесту");
                }

                if (testResult.IsPassed == true)
                {
                    throw new Exception("Этот тест уже завершен");
                }

                var testQuestions = await _context.Questions
                    .Include(q => q.Answers)
                    .Where(q => q.TestId == testResult.TestId)
                    .ToListAsync(cancellationToken);

                var totalQuestions = testQuestions.Count;
                var totalScore = 0;
                var questionResults = new List<QuestionResultDto>();

                var userAnswersByQuestion = request.SelectedAnswers
                    .GroupBy(sa => sa.QuestionId)
                    .ToDictionary(g => g.Key, g => g.Select(sa => sa.AnswerId).ToList());

                foreach (var question in testQuestions)
                {
                    var questionResult = CalculateQuestionScore(question, userAnswersByQuestion);
                    totalScore += questionResult.Score;
                    questionResults.Add(questionResult);
                }

                var overallScore = totalQuestions > 0
                    ? (int)Math.Round(totalScore / (double)totalQuestions, 0)
                    : 0;

                var isPassed = overallScore >= 80;

                testResult.Score = overallScore;
                testResult.IsPassed = isPassed;
                testResult.CompletedAt = request.CompletedAt;

                if (isPassed)
                {
                    await SaveUserAnswers(testResult, request, cancellationToken);
                }

                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                await CheckAllTestsInMaterial(testResult.ProgressMaterialId, cancellationToken);

                var totalCorrectAnswers = questionResults.Sum(q =>
    q.CorrectAnswers.Count(a => a.IsSelectedByUser));

                return new CompleteTestResponseDto
                {
                    IsPassed = isPassed,
                    Score = overallScore,
                    TotalQuestions = totalQuestions,
                    QuestionResults = questionResults,
                    TestId = testResult.TestId,
                    TestTitle = testResult.Test?.Title ?? "Тест",
                    CorrectAnswers = totalCorrectAnswers

                };
            }

        private QuestionResultDto CalculateQuestionScore(
            Question question,
            Dictionary<Guid, List<Guid>> userAnswersByQuestion)
        {
            var correctAnswers = question.Answers
                .Where(a => a.IsCorrect)
                .ToList();

            var totalCorrectInQuestion = correctAnswers.Count;

            var userAnswerIds = userAnswersByQuestion.ContainsKey(question.Id)
                ? userAnswersByQuestion[question.Id]
                : new List<Guid>();

            var selectedAnswers = question.Answers
                .Where(a => userAnswerIds.Contains(a.Id))
                .ToList();

            var correctSelected = selectedAnswers.Count(a => a.IsCorrect);
            var incorrectSelected = selectedAnswers.Count(a => !a.IsCorrect);

            double questionScore = 0;

            if (totalCorrectInQuestion > 0)
            {
                var scoreMultiplier = Math.Max(0, correctSelected - incorrectSelected);
                questionScore = (scoreMultiplier * 100.0) / totalCorrectInQuestion;
                questionScore = Math.Round(questionScore, 0);
            }

            return new QuestionResultDto
            {
                QuestionId = question.Id,
                QuestionText = question.Text,
                Score = (int)questionScore,
                IsFullyCorrect = questionScore >= 99.9,

                CorrectAnswers = correctAnswers.Select(a => new AnswerResultDto
                {
                    AnswerId = a.Id,
                    AnswerText = a.Text,
                    IsSelectedByUser = userAnswerIds.Contains(a.Id)
                }).ToList(),

                IncorrectAnswersSelected = selectedAnswers
                    .Where(a => !a.IsCorrect)
                    .Select(a => new AnswerResultDto
                    {
                        AnswerId = a.Id,
                        AnswerText = a.Text,
                        IsSelectedByUser = true
                    }).ToList(),

                CorrectAnswersMissed = correctAnswers
                    .Where(a => !userAnswerIds.Contains(a.Id))
                    .Select(a => new AnswerResultDto
                    {
                        AnswerId = a.Id,
                        AnswerText = a.Text,
                        IsSelectedByUser = false
                    }).ToList()
            };
        }

        private async Task SaveUserAnswers(
            TestResult testResult,
            CreateAnswersUserCommand request,
            CancellationToken cancellationToken)
        {
            foreach (var answer in request.SelectedAnswers)
            {
                var exists = await _context.AnswersUsers
                    .AnyAsync(au => au.TestResultId == testResult.Id
                        && au.QuestionId == answer.QuestionId
                        && au.AnswerId == answer.AnswerId,
                        cancellationToken);

                if (!exists)
                {
                    var answersUser = new AnswersUser
                    {
                        Id = Guid.NewGuid(),
                        UserId = request.CurrentUserId,
                        AnswerId = answer.AnswerId,
                        QuestionId = answer.QuestionId,
                        TestResultId = testResult.Id,
                    };

                    await _context.AnswersUsers.AddAsync(answersUser, cancellationToken);
                }
            }
        }

        private async Task CheckAllTestsInMaterial(Guid progressMaterialId, CancellationToken cancellationToken)
        {
            var progressMaterial = await _context.ProgressMaterials
                .Include(pm => pm.ProgressModule)
                    .ThenInclude(pm => pm.ProgressUser)
                .FirstOrDefaultAsync(pm => pm.Id == progressMaterialId, cancellationToken);

            if (progressMaterial == null) return;

            var material = await _context.Materials
                .Include(m => m.Tests)
                .FirstOrDefaultAsync(m => m.Id == progressMaterial.MaterialId, cancellationToken);

            if (material?.Tests == null || !material.Tests.Any())
            {
                progressMaterial.Status = "Завершена";
                await CheckProgressModule(progressMaterial, cancellationToken);
                return;
            }

            var testResults = await _context.TestResults
                .Where(tr => tr.ProgressMaterialId == progressMaterialId)
                .ToListAsync(cancellationToken);

            var allTestsPassed = true;
            foreach (var test in material.Tests)
            {
                var testPassed = testResults
                    .Where(tr => tr.TestId == test.Id)
                    .Any(tr => tr.IsPassed);

                if (!testPassed)
                {
                    allTestsPassed = false;
                    break;
                }
            }

            if (allTestsPassed && progressMaterial.Status != "Завершен")
            {
                progressMaterial.Status = "Завершена";

                await CheckProgressModule(progressMaterial, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        private async Task CheckProgressModule(ProgressMaterial progressMaterial, CancellationToken cancellationToken)
        {
            var progressModule = progressMaterial.ProgressModule;

            var moduleMaterials = await _context.ProgressMaterials
                .Where(pm => pm.ProgressModuleId == progressModule.Id)
                .ToListAsync(cancellationToken);

            var allMaterialsCompleted = moduleMaterials.All(pm => pm.Status == "Завершена");

            if (allMaterialsCompleted && progressModule.Status != "Завершен")
            {
                progressModule.Status = "Завершен";

                await CheckProgressUser(progressModule, cancellationToken);
            }
        }

        private async Task CheckProgressUser(ProgressModule progressModule, CancellationToken cancellationToken)
        {
            var progressUser = progressModule.ProgressUser;

            var courseModules = await _context.ProgressModules
                .Where(pm => pm.ProgressUserId == progressUser.Id)
                .ToListAsync(cancellationToken);

            var allModulesCompleted = courseModules.All(pm => pm.Status == "Завершен");

            if (allModulesCompleted && progressUser.Status != "Завершен")
            {
                progressUser.Status = "Пройден";
                progressUser.FineshedAt = DateTime.UtcNow;
            }
        }
    }
}
