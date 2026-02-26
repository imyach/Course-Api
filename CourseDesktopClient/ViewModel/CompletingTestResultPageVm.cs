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
            if (GetQuestion == null || !GetQuestion.Any())
            {
                CustomMessageBox.ShowError("Нет вопросов для завершения теста");
                return;
            }

            var selectedAnswers = CollectSelectedAnswers();

            if (!IsAllQuestionsAnswered())
            {
                CustomMessageBox.ShowInfo("Вы ответили не на все вопросы");
                return;
            }

            if (CustomMessageBox.ShowYesNo("Вы уверены, что хотите завершить тест?", "Подтверждение") != DialogResult.Yes)
                return;

            await SendTestResultsToServer(selectedAnswers);
        }

        private List<SelectedAnswerDto> CollectSelectedAnswers()
        {
            var selectedAnswers = new List<SelectedAnswerDto>();

            if (GetQuestion == null) return selectedAnswers;

            foreach (var question in GetQuestion)
            {
                if (question.Answers == null) continue;

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

        private bool IsAllQuestionsAnswered()
        {
            if (GetQuestion == null) return false;

            foreach (var question in GetQuestion)
            {
                if (question.Answers?.Any(a => a.IsSelected) != true)
                {
                    return false;
                }
            }

            return true;
        }

        private async Task SendTestResultsToServer(List<SelectedAnswerDto> selectedAnswers)
        {
            try
            {
                var testResultRequest = new CompleteTestRequestDto
                {
                    TestResultId = localIdTestResult,
                    SelectedAnswers = selectedAnswers,
                    CompletedAt = DateTime.UtcNow
                };

                var result = await courseApiClient.CompleteTestAsync(testResultRequest);

                if (result != null)
                {
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

    public class CompleteTestRequestDto
    {
        public Guid TestResultId { get; set; }
        public List<SelectedAnswerDto> SelectedAnswers { get; set; } = new();
        public DateTime CompletedAt { get; set; }
    }
}
