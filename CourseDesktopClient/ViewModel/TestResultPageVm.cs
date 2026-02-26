using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using CourseDesktopClient.UI.Elements.ElementVM;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    public class TestResultPageVm : NavigationVm
    {
        private readonly ICourseApiClient _courseApiClient;

        private ObservableCollection<AnswerUserElementVm> _answerUserElements = new();
        public ObservableCollection<AnswerUserElementVm> AnswerUserElements
        {
            get => _answerUserElements;
            set
            {
                _answerUserElements = value;
                OnPropertyChanged();
            }
        }

        private TestHistoryVm _testHistory;
        public TestHistoryVm TestHistory
        {
            get => _testHistory;
            set
            {
                _testHistory = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasData));
                OnPropertyChanged(nameof(TestTitle));
                OnPropertyChanged(nameof(TestStatus));
                OnPropertyChanged(nameof(TestStatusColor));
                OnPropertyChanged(nameof(TestStatusIcon));
                OnPropertyChanged(nameof(ScoreText));
                OnPropertyChanged(nameof(PassingScoreText));
                OnPropertyChanged(nameof(CompletionDateText));
                OnPropertyChanged(nameof(IsPassed));
            }
        }

        public bool HasData => TestHistory != null;
        public string TestTitle => TestHistory?.TestTitle ?? "Тест";
        public bool IsPassed => TestHistory?.IsTestPassed == true;
        public string TestStatus => IsPassed ? "ПРОЙДЕН" : "НЕ ПРОЙДЕН";

        public string TestStatusIcon => IsPassed ? "✓" : "✗";

        public string TestStatusColor => IsPassed ? "#4CAF50" : "#F44336";

        public string ScoreText => TestHistory != null ? $"{TestHistory.BestScore}%" : "0%";
        public string PassingScoreText => TestHistory != null ? $"Проходной балл: {TestHistory.PassingScore}%" : "";

        public string CompletionDateText => TestHistory?.CompletedAt != null
            ? $"Завершен: {TestHistory.CompletedAt.Value:dd.MM.yyyy HH:mm}"
            : "Не завершен";

        public ICommand PassMaterialCommand { get; set; }

        public TestResultPageVm(INavigationService navigationService, ICourseApiClient courseApiClient) : base(navigationService)
        {
            _courseApiClient = courseApiClient;

            PassMaterialCommand = new RelayCommand(async _ =>
            {
                await navigationService.NavigateToProgressMaterial(
                    CompletingModulePageVm.localIdProgressModule,
                    CompletingMaterialPageVm.localIdProgressMaterial);
            });
        }

        public async Task LoadTestHistory(Guid testResultId)
        {
            try
            {
                var testHistory = await _courseApiClient.GetAnswersUsersAsync(testResultId);

                if (testHistory != null)
                {
                    TestHistory = testHistory;

                    AnswerUserElements.Clear();

                    if (testHistory.Questions != null)
                    {
                        foreach (var question in testHistory.Questions)
                        {
                            var element = new AnswerUserElementVm
                            {
                                QuestionText = question.QuestionText,
                                Score = question.Score,
                                IsCorrect = question.IsCorrect
                            };

                            element.CorrectAnswers.Clear();
                            foreach (var answer in question.Answers.Where(a => a.IsCorrect))
                            {
                                element.CorrectAnswers.Add(new AnswerHistoryItemVm
                                {
                                    AnswerText = answer.AnswerText,
                                    IsCorrect = answer.IsCorrect
                                });
                            }

                            element.UserAnswers.Clear();
                            foreach (var answer in question.Answers.Where(a => a.IsSelectedByUser))
                            {
                                element.UserAnswers.Add(new AnswerHistoryItemVm
                                {
                                    AnswerText = answer.AnswerText,
                                    IsCorrect = answer.IsCorrect,
                                    IsSelectedByUser = answer.IsSelectedByUser
                                });
                            }

                            AnswerUserElements.Add(element);
                        }
                    }
                }
                else
                {
                    CustomMessageBox.ShowError("Не удалось загрузить историю теста");
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError($"Ошибка при загрузке истории: {ex.Message}");
            }
        }
    }
}