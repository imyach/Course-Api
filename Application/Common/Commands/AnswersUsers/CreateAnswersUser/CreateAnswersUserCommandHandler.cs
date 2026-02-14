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
        private readonly ILogger<CreateAnswersUserCommandHandler> _logger;

        public CreateAnswersUserCommandHandler(
            ICoursesDbContext context,
            ILogger<CreateAnswersUserCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CompleteTestResponseDto> Handle(CreateAnswersUserCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                // 1. Находим TestResult (только этот конкретный тест)
                var testResult = await _context.TestResults
                    .Include(tr => tr.Test)
                    .Include(tr => tr.ProgressMaterial)
                    .FirstOrDefaultAsync(tr => tr.Id == request.TestResultId, cancellationToken);

                if (testResult == null)
                {
                    throw new Exception($"TestResult с ID {request.TestResultId} не найден");
                }

                // 2. Проверяем права доступа
                if (testResult.UserId != request.CurrentUserId)
                {
                    throw new UnauthorizedAccessException("Нет доступа к этому тесту");
                }

                // 3. Проверяем, не завершен ли уже тест
                if (testResult.IsPassed == true)
                {
                    throw new Exception("Этот тест уже завершен");
                }

                // 4. Получаем вопросы ТОЛЬКО для этого теста
                var testQuestions = await _context.Questions
                    .Include(q => q.Answers)
                    .Where(q => q.TestId == testResult.TestId)
                    .ToListAsync(cancellationToken);

                var totalQuestions = testQuestions.Count;
                var totalScore = 0;
                var questionResults = new List<QuestionResultDto>();

                // Группируем ответы пользователя по вопросам
                var userAnswersByQuestion = request.SelectedAnswers
                    .GroupBy(sa => sa.QuestionId)
                    .ToDictionary(g => g.Key, g => g.Select(sa => sa.AnswerId).ToList());

                // 5. Проверяем каждый вопрос
                foreach (var question in testQuestions)
                {
                    var questionResult = CalculateQuestionScore(question, userAnswersByQuestion);
                    totalScore += questionResult.Score;
                    questionResults.Add(questionResult);
                }

                // 6. Вычисляем процент для ЭТОГО теста
                var overallScore = totalQuestions > 0
                    ? (int)Math.Round(totalScore / (double)totalQuestions, 0)
                    : 0;

                var isPassed = overallScore >= 80;

                // 7. Сохраняем результат в TestResult (именно здесь хранится процент!)
                testResult.Score = overallScore;
                testResult.IsPassed = isPassed;
                testResult.CompletedAt = request.CompletedAt;

                // 8. Если тест пройден - создаем записи в AnswersUser
                if (isPassed)
                {
                    await SaveUserAnswers(testResult, request, cancellationToken);
                }

                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                // 9. Проверяем, все ли тесты материала пройдены (НО НЕ МЕНЯЕМ PROGRESSMATERIAL)
                await CheckAllTestsInMaterial(testResult.ProgressMaterialId, cancellationToken);

                var totalCorrectAnswers = questionResults.Sum(q =>
    q.CorrectAnswers.Count(a => a.IsSelectedByUser));

                // 10. Формируем ответ только для ЭТОГО теста
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
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Ошибка при завершении теста {TestResultId}", request.TestResultId);
                throw;
            }
        }

        private QuestionResultDto CalculateQuestionScore(
            Question question,
            Dictionary<Guid, List<Guid>> userAnswersByQuestion)
        {
            // Получаем все правильные ответы для этого вопроса
            var correctAnswers = question.Answers
                .Where(a => a.IsCorrect)
                .ToList();

            var totalCorrectInQuestion = correctAnswers.Count;

            // Получаем ответы пользователя на этот вопрос
            var userAnswerIds = userAnswersByQuestion.ContainsKey(question.Id)
                ? userAnswersByQuestion[question.Id]
                : new List<Guid>();

            // Находим выбранные пользователем ответы
            var selectedAnswers = question.Answers
                .Where(a => userAnswerIds.Contains(a.Id))
                .ToList();

            // Подсчитываем:
            var correctSelected = selectedAnswers.Count(a => a.IsCorrect);
            var incorrectSelected = selectedAnswers.Count(a => !a.IsCorrect);

            // Очки за вопрос (от 0 до 100)
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
                // Проверяем, не сохранен ли уже этот ответ
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

            // Получаем ВСЕ тесты для этого материала
            var material = await _context.Materials
                .Include(m => m.Tests)
                .FirstOrDefaultAsync(m => m.Id == progressMaterial.MaterialId, cancellationToken);

            if (material?.Tests == null || !material.Tests.Any())
            {
                // Если у материала нет тестов, он считается пройденным
                progressMaterial.Status = "Завершена";
                await CheckProgressModule(progressMaterial, cancellationToken);
                return;
            }

            // Получаем все TestResult для этого материала
            var testResults = await _context.TestResults
                .Where(tr => tr.ProgressMaterialId == progressMaterialId)
                .ToListAsync(cancellationToken);

            // Проверяем, все ли тесты пройдены
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

            // Обновляем статус материала ТОЛЬКО если все тесты пройдены
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

            // Получаем все материалы этого модуля
            var moduleMaterials = await _context.ProgressMaterials
                .Where(pm => pm.ProgressModuleId == progressModule.Id)
                .ToListAsync(cancellationToken);

            // Проверяем, все ли материалы пройдены
            var allMaterialsCompleted = moduleMaterials.All(pm => pm.Status == "Завершена");

            if (allMaterialsCompleted && progressModule.Status != "Завершен")
            {
                progressModule.Status = "Завершен";

                // Проверяем ProgressUser (весь курс)
                await CheckProgressUser(progressModule, cancellationToken);
            }
        }

        private async Task CheckProgressUser(ProgressModule progressModule, CancellationToken cancellationToken)
        {
            var progressUser = progressModule.ProgressUser;

            // Получаем все модули этого пользователя
            var courseModules = await _context.ProgressModules
                .Where(pm => pm.ProgressUserId == progressUser.Id)
                .ToListAsync(cancellationToken);

            // Проверяем, все ли модули пройдены
            var allModulesCompleted = courseModules.All(pm => pm.Status == "Завершен");

            if (allModulesCompleted && progressUser.Status != "Завершен")
            {
                progressUser.Status = "Пройден";
                progressUser.FineshedAt = DateTime.UtcNow;
            }
        }
    }
}
