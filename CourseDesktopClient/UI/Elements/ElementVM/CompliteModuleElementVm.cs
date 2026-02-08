using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CourseDesktopClient.UI.Elements.ElementVM
{
    class CompliteModuleElementVm(ProgressModuleDto progressModuleDto)
    {
        public Guid Id => progressModuleDto.Id;
        public string Status  => progressModuleDto.Status;
        public DateTime? StartedAt => progressModuleDto.StartedAt;
        public string ModuleTitle => progressModuleDto.Module.Title;
        public string ModuleDescription => progressModuleDto.Module.Description;

        public string ContentButton
        {
            get
            {
                if (progressModuleDto.StartedAt == null)
                    return "Начать";
                else
                    return "Продолжить";
            }
        }
    }
}
