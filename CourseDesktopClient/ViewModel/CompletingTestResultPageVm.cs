using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.UI.Elements.ElementVM;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace CourseDesktopClient.ViewModel
{
    public class CompletingTestResultPageVm : NavigationVm
    {
        private IList<PassQuestionElementVm>? _getQuestion;
        public IList<PassQuestionElementVm>? GetQuestion { get => _getQuestion; set { _getQuestion = value; OnPropertyChanged(nameof(GetQuestion)); } }

        private string _title = string.Empty;
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }

        private string? _description;
        public string? Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }

        private readonly ICourseApiClient courseApiClient;
        private readonly INavigationService navigationService;
        public static Guid localIdProgressMaterial;
        public static Guid localIdTestResult;
        public ICommand PassMaterialCommand { get; set; }
        public ICommand СompletionTestCommand { get; set; }

        private Visibility _visibleDescriptionBorder { get; set; }
        public Visibility VisibleDescriptionBorder
        {
            get => _visibleDescriptionBorder;
            set { _visibleDescriptionBorder = value; OnPropertyChanged(); }
        }

        public CompletingTestResultPageVm(INavigationService navigationService, ICourseApiClient courseApiClient) : base(navigationService) 
        {
            this.navigationService = navigationService;
            this.courseApiClient = courseApiClient;
            PassMaterialCommand = new RelayCommand(async sender =>
            {
                if (CustomMessageBox.ShowYesNo("Вы действительно хотите выйти\nВаш прогресс тогда не сохранится", "Предупреждение") == DialogResult.Yes)
                    await navigationService.NavigateToProgressMaterial(CompletingMaterialPageVm.localIdProgressModule, localIdProgressMaterial);
            });
            СompletionTestCommand = new RelayCommand(async sender => {await СompletionTestAsync(sender); });
        }

        public async Task LoadTestResult(Guid progressMaterialId, Guid testResultId)
        {
            localIdProgressMaterial = progressMaterialId;
            localIdTestResult = testResultId;

            var infoTestResult = await courseApiClient.GetTestResultByIdAsync(testResultId);

            Title = infoTestResult.Test.Title;
            Description = infoTestResult.Test.Description;

            var questions = await courseApiClient.GetQuestionsAsync(infoTestResult.Test.Id);

            var questionViewModel = questions.Questions.Select(x => new PassQuestionElementVm(x)).ToList();
            GetQuestion = questionViewModel;

            VisibleDescriptionBorder = string.IsNullOrEmpty(Description)
                ? Visibility.Collapsed
                : Visibility.Visible;
 
        }

        private async Task СompletionTestAsync(object? sender)
        {
            // 1. Проверяем есть ли вопросы
            if (GetQuestion == null || !GetQuestion.Any())
            {
                CustomMessageBox.ShowError("Нет вопросов для завершения теста");
                return;
            }

            // 2. Собираем выбранные ответы
            var selectedAnswers = CollectSelectedAnswers();

            // 3. Проверяем, на все ли вопросы ответили
            if (!IsAllQuestionsAnswered())
            {
                CustomMessageBox.ShowInfo("Вы ответили не на все вопросы");
                return;
            }

            // 4. Подтверждение завершения
            if (CustomMessageBox.ShowYesNo("Вы уверены, что хотите завершить тест?", "Подтверждение") != DialogResult.Yes)
                return;

            // 5. Отправляем данные на сервер
            await SendTestResultsToServer(selectedAnswers);
        }

        private List<SelectedAnswerDto> CollectSelectedAnswers()
        {
            var selectedAnswers = new List<SelectedAnswerDto>();

            if (GetQuestion == null) return selectedAnswers;

            foreach (var question in GetQuestion)
            {
                if (question.Answers == null) continue;

                // Находим выбранные ответы для этого вопроса
                var selectedForQuestion = question.Answers
                    .Where(a => a.IsSelected)
                    .Select(a => new SelectedAnswerDto
                    {
                        QuestionId = question.Id,
                        AnswerId = a.Id,
                        AnswerText = a.TextAnswer
                    })
                    .ToList();

                selectedAnswers.AddRange(selectedForQuestion);
            }

            return selectedAnswers;
        }

        // Проверка на все ли вопросы отвечены
        private bool IsAllQuestionsAnswered()
        {
            if (GetQuestion == null) return false;

            foreach (var question in GetQuestion)
            {
                // Если у вопроса нет выбранных ответов
                if (question.Answers?.Any(a => a.IsSelected) != true)
                {
                    return false;
                }
            }

            return true;
        }

        // Отправка результатов на сервер
        private async Task SendTestResultsToServer(List<SelectedAnswerDto> selectedAnswers)
        {
            try
            {
                // Создаем DTO для отправки
                var testResultRequest = new CompleteTestRequestDto
                {
                    TestResultId = localIdTestResult,
                    SelectedAnswers = selectedAnswers,
                    CompletedAt = DateTime.UtcNow
                };

                // Отправляем на сервер
                var result = await courseApiClient.CompleteTestAsync(testResultRequest);

                if (result != null)
                {
                    // Показываем результат с сервера
                    ShowTestResult(result);
                }
                else
                {
                    CustomMessageBox.ShowError("Не удалось завершить тест. Попробуйте снова.");
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError($"Ошибка при отправке результатов: {ex.Message}");
            }
        }

        // Показ результатов теста
        private void ShowTestResult(CompleteTestResponseDto result)
        {
            if (result.IsPassed)
            {
                CustomMessageBox.ShowInfo(
                    $"✅ ТЕСТ ПРОЙДЕН!\n\n" +
                    $"Результат: {result.Score}%\n" +
                    $"Правильных ответов: {result.CorrectAnswers}/{result.TotalQuestions}",
                    "Тест пройден");

                navigationService.NavigateToResultsTest(localIdTestResult);
            }
            else
            {
                CustomMessageBox.ShowInfo(
                    $"❌ ТЕСТ НЕ ПРОЙДЕН\n\n" +
                    $"Результат: {result.Score}%\n" +
                    $"Нужно набрать 80%\n" +
                    $"Правильных ответов: {result.CorrectAnswers}/{result.TotalQuestions}",
                    "Тест не пройден");
            }
        }
    }
    public class SelectedAnswerDto
    {
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
        public string AnswerText { get; set; } = string.Empty;
    }

    // DTO для завершения теста
    public class CompleteTestRequestDto
    {
        public Guid TestResultId { get; set; }
        public List<SelectedAnswerDto> SelectedAnswers { get; set; } = new();
        public DateTime CompletedAt { get; set; }
    }
}
