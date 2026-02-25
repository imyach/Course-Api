using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.UI.Elements.ElementVM;
using CourseDesktopClient.Utilities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace CourseDesktopClient.ViewModel
{
    public class CreateQuestionPageVm : NavigationVm
    {
        private IList<CreateAnswerElementVm>? _getAnswer;
        public IList<CreateAnswerElementVm>? GetAnswer { get => _getAnswer; set { _getAnswer = value; OnPropertyChanged(nameof(GetAnswer)); } }

        private string _textOfQuestion;
        public string TextOfQuestion
        {
            get { return _textOfQuestion; }
            set { _textOfQuestion = value; OnPropertyChanged(); CheckEnableSave(); }
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

        private Visibility _visibleAddAnswerButton;
        public Visibility VisibleAddAnswerButton
        {
            get { return _visibleAddAnswerButton; }
            set
            {
                _visibleAddAnswerButton = value;
                OnPropertyChanged();
            }
        }

                private Visibility? _visibleMisstake = Visibility.Collapsed;
        public Visibility? VisibleMisstake
        {

            get { return _visibleMisstake; }
            set { _visibleMisstake = value; OnPropertyChanged(); }
        }
        private string? _mistakeText = string.Empty;
        public string? MistakeText
        {
            get { return _mistakeText; }
            set { _mistakeText = value; OnPropertyChanged(); }
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

        public static Guid questionId;
        private Guid testId;

        public ICommand CreateAnswerCommand { get; set; }
        public ICommand SaveUpdateCommand { get; set; }
        public ICommand DeleteQuestionCommand { get; set; }
        public ICommand LocalSaveAnswerCommand { get; set; }
        public ICommand LocalDeleteAnswerCommand { get; set; }
        public ICommand LocalCreataeTestCommand { get; set; }

        public CreateQuestionPageVm(INavigationService navigationService, ICourseApiClient courseApiClient) : base(navigationService)
        {
            this.courseApiClient = courseApiClient;

            LocalCreataeTestCommand = new RelayCommand(async sender =>
            {
                await navigationService.NavigateToCreateTest(CreateMaterialPageVm.materialId, testId);
            });

            CreateAnswerCommand = new RelayCommand(async sender =>
            {
                var newAnswer = new AnswerDto() { Text = "Пустой ответ", IsCorrect = false, QuestionId = questionId};

                var idNewAnswer = await courseApiClient.CreateAnswerAsync(newAnswer);

                var answers = await courseApiClient.GetAnswersAsync(questionId);
                var answersViewModel = answers.Answers.Select(x => new CreateAnswerElementVm(x)).ToList();

                await LoadQuestion(questionId, testId);
            });

            LocalSaveAnswerCommand = new RelayCommand(async sender =>
            {
                var answerElementVm = sender as CreateAnswerElementVm;

                if (string.IsNullOrEmpty(answerElementVm.Text))
                {
                    answerElementVm.MistakeAnswerText = "Введите ответ";
                    answerElementVm.VisibleAnswerMisstake = Visibility.Visible;
                    return;
                }
                else if (answerElementVm.Text.Length >250)
                {
                    answerElementVm.MistakeAnswerText = "Ответ не может превышать 250 символов";
                    answerElementVm.VisibleAnswerMisstake = Visibility.Visible;
                    return;
                }
                else
                {
                    answerElementVm.MistakeAnswerText = string.Empty;
                    answerElementVm.VisibleAnswerMisstake = Visibility.Collapsed;
                }

                var updateAnswer = new AnswerDto
                {
                    Id = answerElementVm.Id,
                    Text = answerElementVm.Text,
                    IsCorrect = answerElementVm.IsCorrect,
                };

                await courseApiClient.UpdateAnswerAsync(updateAnswer);
                await LoadQuestion(questionId, testId);
                CustomMessageBox.ShowInfo("Изменения сохранены");
            });

            LocalDeleteAnswerCommand = new RelayCommand(async sender =>
            {
                if (CustomMessageBox.ShowYesNo("Вы дествительно хотите удалить данный ответ? \nПосле этого произойдет автоматическое сохранеие!") == DialogResult.Yes)
                {
                    await courseApiClient.DeleteAnswerAsync((sender as CreateAnswerElementVm).Id);
                    await LoadQuestion(questionId, testId);
                    CustomMessageBox.ShowInfo("Ответ удален");
                }
            });

            SaveUpdateCommand = new RelayCommand(async sender =>
            {

                if (string.IsNullOrEmpty(TextOfQuestion))
                {
                    MistakeText = "Введите вопрос";
                    VisibleMisstake = Visibility.Visible;
                    return;
                }
                else
                {
                    MistakeText = string.Empty;
                    VisibleMisstake = Visibility.Collapsed;
                }

                var updateQuestion = new QuestionDto
                {
                    Id = questionId,
                    Text = TextOfQuestion,
                };

                await courseApiClient.UpdateQuestionAsync(updateQuestion);
                CustomMessageBox.ShowInfo("Изменения сохранены");
            });
            DeleteQuestionCommand = new RelayCommand(async sender =>
            {
                if (CustomMessageBox.ShowYesNo("Вы дествительно хотите удалить данный вопрос? \nПосле этого произойдет автоматическое сохранеие!") == DialogResult.Yes)
                {
                    await courseApiClient.DeleteQuestionAsync(questionId);
                    await navigationService.NavigateToCreateTest(CreateMaterialPageVm.materialId, testId);
                    CustomMessageBox.ShowInfo("Вопрос удален");
                }
            });
        }

        public async Task LoadQuestion(Guid idQuestion, Guid idTest)
        {
            MistakeText = string.Empty;
            VisibleMisstake = Visibility.Collapsed;
            testId = idTest;
            if (idQuestion != default)
            {
                questionId = idQuestion;
                var question = await courseApiClient.GetQuestionAsync(idQuestion);

                TextOfQuestion = question.Text;

                var answers = await courseApiClient.GetAnswersAsync(idQuestion);
                var answersViewModel = answers.Answers.Select(x => new CreateAnswerElementVm(x)).ToList();
                
                GetAnswer = answersViewModel;

                VisibleAddAnswerButton = GetAnswer.Count > 0
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                VisibleEmptyPage = GetAnswer.Count <= 0
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                CheckEnableSave();
            }
            else
            {
                var newQuestion = new QuestionDto() {Text = "Новый вопрос", TestId = testId};

                var idNewQuestion = await courseApiClient.CreateQuestionAsync(newQuestion);

                var question = await courseApiClient.GetQuestionAsync(idNewQuestion ?? throw new Exception());
                questionId = question.Id;
                TextOfQuestion = question.Text;

                VisibleEmptyPage = Visibility.Visible;
                VisibleAddAnswerButton = Visibility.Collapsed;
            }
        }


        private void CheckEnableSave()
        {
            if (GetAnswer?.Count > 0 && !string.IsNullOrEmpty(TextOfQuestion))
                IsSaveEnable = true;
            else
                IsSaveEnable = false;
        }
    }
}
