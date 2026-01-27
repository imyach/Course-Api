using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.UI.Elements.ElementVM
{
    public class CreateMaterialElementVm(MaterialDto materialDto)
    {
        public Guid Id => materialDto.Id;
        public string Title => materialDto.Title;
        public string? Description => materialDto.Description;
        public int Order => materialDto.Order;
    }
}
