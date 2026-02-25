using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CourseDesktopClient.UI.Elements.ElementVM
{
    public class TestResultElementVm(TestResultDto testResultDto)
    {
        public Guid Id => testResultDto.Id;
        public bool IsPassed => testResultDto.IsPassed;
        public DateTime? CompletedAt => testResultDto.CompletedAt;
        public int Score => testResultDto.Score;
        public string TestTitle => testResultDto.Test.Title;
        public string TestDescription => testResultDto.Test.Description;

        public string StatusText => IsPassed ? "Завершен" : "Не завершен";
        public Visibility ButtonVisible => IsPassed == false ? Visibility.Visible : Visibility.Collapsed;
    }
}
