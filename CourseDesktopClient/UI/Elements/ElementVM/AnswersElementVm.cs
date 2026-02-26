using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace CourseDesktopClient.UI.Elements.ElementVM
{
    public class AnswersElementVm : ViewModelBase
    {
        private readonly AnswerDto _answerDto;
        private bool _isSelected;

        public Guid Id => _answerDto.Id;
        public string TextAnswer => _answerDto.Text ?? string.Empty;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public AnswersElementVm(AnswerDto answerDto)
        {
            _answerDto = answerDto ?? throw new ArgumentNullException(nameof(answerDto));
        }
    }
}
