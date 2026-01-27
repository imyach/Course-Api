using CourseDesktopClient.Models.DtosModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CourseDesktopClient.Models.DtosModel.EntitiesLists
{
    public class TestsDto
    {
        [JsonPropertyName("tests")]
        public IList<TestDto>? Tests { get; set; }
    }
}
