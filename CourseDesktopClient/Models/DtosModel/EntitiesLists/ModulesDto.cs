using CourseDesktopClient.Models.DtosModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CourseDesktopClient.Models.DtosModel.EntitiesLists
{
    public class ModulesDto
    {
        [JsonPropertyName("modules")]
        public IList<ModuleDto>? Modules { get; set; }
    }
}
