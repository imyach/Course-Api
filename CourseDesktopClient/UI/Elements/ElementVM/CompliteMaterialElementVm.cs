using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.UI.Elements.ElementVM
{
    public class CompliteMaterialElementVm(ProgressMaterialDto progressMaterialDto)
    {
        public Guid Id => progressMaterialDto.Id;
        public string Status => progressMaterialDto.Status;
        public DateTime? StartedAt => progressMaterialDto.StartedAt;
        public string MaterialTitle => progressMaterialDto.Material.Title;
        public string MaterialDescription => progressMaterialDto.Material.Description;

        public string ContentButton
        {
            get
            {
                if (progressMaterialDto.StartedAt == null)
                    return "Начать";
                else
                    return "Продолжить";
            }
        }
    }
}
