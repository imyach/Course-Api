using CourseDesktopClient.Models.DtosModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CourseDesktopClient.UI.Elements.ElementVM
{
    public class CreateModulePanelElementVm(ModuleDto moduleDto)
    {
        public Guid Id  => moduleDto.Id;
        public string Title => moduleDto.Title;
        public string? Description => moduleDto.Description;
        public int Order => moduleDto.Order;
    }
}
