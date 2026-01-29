using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using CourseDesktopClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.UI.Elements.ElementVM
{
    public class CreateAnswerElementVm (AnswerDto answerDto, INavigationService navigationService = default) : NavigationVm(navigationService)
    {
        public Guid Id => answerDto.Id;
        public string Text
        {
            get => answerDto.Text;
            set
            {
                if (answerDto.Text != value)
                {
                    answerDto.Text = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsCorrect
        {
            get => answerDto.IsCorrect;
            set
            {
                if (answerDto.IsCorrect != value)
                {
                    answerDto.IsCorrect = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
