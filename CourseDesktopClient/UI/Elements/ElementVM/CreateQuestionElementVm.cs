using CourseDesktopClient.Models.DtosModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.UI.Elements.ElementVM
{
    public class CreateQuestionElementVm(QuestionDto questionDto)
    {
        public Guid Id => questionDto.Id;
        public string Text => questionDto.Text;
    }
}
