using CourseDesktopClient.Models.DtosModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.UI.Elements.ElementVM
{
    public class CreateTestElementVm(TestDto testDto)
    {
        public Guid Id => testDto.Id;
        public string Title => testDto.Title;
        public string Description => testDto.Description;

    }
}
