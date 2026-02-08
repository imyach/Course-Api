using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.UI.Elements.ElementVM
{
    public class PassQuestionElementVm : ViewModelBase
    {
        private readonly QuestionDto _question;
        private IList<AnswersElementVm>? _answers;

        public Guid Id => _question.Id;
        public string TextQuestion => _question.Text ?? string.Empty;
        public IList<AnswersElementVm>? Answers
        {
            get => _answers;
            set
            {
                _answers = value;
                OnPropertyChanged(nameof(Answers));
            }
        }

        public PassQuestionElementVm(QuestionDto question)
        {
            _question = question ?? throw new ArgumentNullException(nameof(question));

            // Создаем VM для ответов
            if (question.Answers != null)
            {
                Answers = question.Answers
                    .Select(a => new AnswersElementVm(a))
                    .ToList();
            }
        }
    }
}
