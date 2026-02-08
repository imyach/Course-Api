using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.UI.Elements.ElementVM;
using CourseDesktopClient.Utilities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    public class CreateTestPageVm : NavigationVm
    {
        private IList<CreateQuestionElementVm>? _getQuestion;
        public IList<CreateQuestionElementVm>? GetQuestion { get => _getQuestion; set { _getQuestion = value; OnPropertyChanged(nameof(GetQuestion)); } }

        private string _titleOfTest = string.Empty;
        public string TitleOfTest
        {
            get { return _titleOfTest; }
            set { _titleOfTest = value; OnPropertyChanged(); CheckEnableSave(); }
        }

        private string? _descriptionOfTest;
        public string? DescriptionOfTest
        {
            get { return _descriptionOfTest; }
            set { _descriptionOfTest = value; OnPropertyChanged(); CheckEnableSave(); }
        }

        private bool _isSaveEnable;
        public bool IsSaveEnable
        {
            get { return _isSaveEnable; }
            set
            {
                _isSaveEnable = value;
                OnPropertyChanged();
            }
        }

        private Visibility _visibleAddQuestionButton;
        public Visibility VisibleAddQuestionButton
        {
            get { return _visibleAddQuestionButton; }
            set
            {
                _visibleAddQuestionButton = value;
                OnPropertyChanged();
            }
        }

        private Visibility _visibleEmptyPage;
        public Visibility VisibleEmptyPage
        {
            get { return _visibleEmptyPage; }
            set
            {
                _visibleEmptyPage = value;
                OnPropertyChanged();
            }
        }
        private readonly ICourseApiClient courseApiClient;

        public static Guid testId;
        private Guid materialId;

        public ICommand CreateQuestionCommand { get; set; }
        public ICommand SaveUpdateCommand { get; set; }
        public ICommand DeleteTestCommand { get; set; }
        public ICommand LocalCreateQuestionCommand { get; set; }
        public ICommand LocalDeleteQuestionCommand { get; set; }
        public ICommand LocalCreataeMaterialCommand { get; set; }


        public CreateTestPageVm(INavigationService navigationService, ICourseApiClient courseApiClient) : base(navigationService)
        {
            this.courseApiClient = courseApiClient;

            LocalCreataeMaterialCommand = new RelayCommand(async sender =>
            {
                await navigationService.NavigateToCreateMaterial(CreateModulePageVm.moduleId, materialId);
            });

            CreateQuestionCommand = new RelayCommand(async sender =>
            {
                await navigationService.NavigateToCreateQuestion(testId);
            });

            LocalCreateQuestionCommand = new RelayCommand(async sender =>
            {
                await navigationService.NavigateToCreateQuestion(testId, (sender as CreateQuestionElementVm).Id);
            });

            LocalDeleteQuestionCommand = new RelayCommand(async sender =>
            {
                if (CustomMessageBox.ShowYesNo("Вы дествительно хотите удалить данный вопрос? \nПосле этого произойдет автоматическое сохранеие!") == DialogResult.Yes)
                {
                    await courseApiClient.DeleteQuestionAsync((sender as CreateQuestionElementVm).Id);
                    await LoadTest(testId, materialId);
                    CustomMessageBox.ShowInfo("Вопрос удален");
                }
            });

            SaveUpdateCommand = new RelayCommand(async sender =>
            {
                var updateTest = new TestDto
                {
                    Id = testId,
                    Title = TitleOfTest,
                    Description = DescriptionOfTest
                };

                await courseApiClient.UpdateTestAsync(updateTest);
                CustomMessageBox.ShowInfo("Изменения сохранены");
            });
            DeleteTestCommand = new RelayCommand(async sender =>
            {
                if (CustomMessageBox.ShowYesNo("Вы дествительно хотите удалить данный тест? \nПосле этого произойдет автоматическое сохранеие!") == DialogResult.Yes)
                {
                    await courseApiClient.DeleteTestAsync(testId);
                    await navigationService.NavigateToCreateMaterial(CreateModulePageVm.moduleId, materialId);
                    CustomMessageBox.ShowInfo("Тест удален");
                }
            });

        }

        public async Task LoadTest(Guid idTest, Guid idMaterial)
        {
            materialId = idMaterial;
            if (idTest != default)
            {
                testId = idTest;
                var test = await courseApiClient.GetTestAsync(idTest);

                TitleOfTest = test.Title;
                DescriptionOfTest = test.Description;

                var questions = await courseApiClient.GetQuestionsAsync(idTest);
                var questionsViewModel = questions.Questions.Select(x => new CreateQuestionElementVm(x)).ToList();
                GetQuestion = questionsViewModel;

                VisibleAddQuestionButton = GetQuestion.Count > 0
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                VisibleEmptyPage = GetQuestion.Count <= 0
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                CheckEnableSave();
            }
            else
            {
                var newTest = new TestDto() { Description = "Описание теста", Title = "Новый тест", MaterialId = materialId };

                var idNewTest = await courseApiClient.CreateTestAsync(newTest);

                var test = await courseApiClient.GetTestAsync(idNewTest ?? throw new Exception());
                testId = test.Id;
                DescriptionOfTest = test.Description;
                TitleOfTest = test.Title;

                VisibleEmptyPage = Visibility.Visible;
                VisibleAddQuestionButton = Visibility.Collapsed;
            }
        }
        private void CheckEnableSave()
        {
            if (GetQuestion?.Count > 0 && !string.IsNullOrEmpty(TitleOfTest))
                IsSaveEnable = true;
            else
                IsSaveEnable = false;
        }
    }
}
