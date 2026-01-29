using CourseDesktopClient.Models.DtosModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CourseDesktopClient.Models.DtosModel.EntitiesLists
{
    public class AnswersDto
    {
        [JsonPropertyName("answers")]
        public IList<AnswerDto>? Answers { get; set; }
    }
}
