using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CourseDesktopClient.UI.Elements.ElementVM
{
    public class AnswerUserElementVm : ViewModelBase
    {
        private string _questionText = string.Empty;
        public string QuestionText
        {
            get => _questionText;
            set
            {
                _questionText = value;
                OnPropertyChanged(nameof(QuestionText));
            }
        }

        private int _score;
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                OnPropertyChanged(nameof(Score));
            }
        }

        private bool _isCorrect;
        public bool IsCorrect
        {
            get => _isCorrect;
            set
            {
                _isCorrect = value;
                OnPropertyChanged(nameof(IsCorrect));
            }
        }

        private ObservableCollection<AnswerHistoryItemVm> _correctAnswers = new();
        public ObservableCollection<AnswerHistoryItemVm> CorrectAnswers
        {
            get => _correctAnswers;
            set
            {
                _correctAnswers = value;
                OnPropertyChanged(nameof(CorrectAnswers));
            }
        }

        private ObservableCollection<AnswerHistoryItemVm> _userAnswers = new();
        public ObservableCollection<AnswerHistoryItemVm> UserAnswers
        {
            get => _userAnswers;
            set
            {
                _userAnswers = value;
                OnPropertyChanged(nameof(UserAnswers));
            }
        }
    }

    public class AnswerHistoryItemVm : ViewModelBase
    {
        private string _answerText = string.Empty;
        public string AnswerText
        {
            get => _answerText;
            set
            {
                _answerText = value;
                OnPropertyChanged(nameof(AnswerText));
            }
        }

        private bool _isCorrect;
        public bool IsCorrect
        {
            get => _isCorrect;
            set
            {
                _isCorrect = value;
                OnPropertyChanged(nameof(IsCorrect));
            }
        }

        private bool _isSelectedByUser;
        public bool IsSelectedByUser
        {
            get => _isSelectedByUser;
            set
            {
                _isSelectedByUser = value;
                OnPropertyChanged(nameof(IsSelectedByUser));
            }
        }
    }
}