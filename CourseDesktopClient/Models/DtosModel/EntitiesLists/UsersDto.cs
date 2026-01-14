using CourseDesktopClient.Models.DtosModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows.Documents;

namespace CourseDesktopClient.Models.DtosModel.EntitiesLists
{
    public class UsersDto
    {
        [JsonPropertyName("users")]
        public IList<UserDto>? Users { get; set; }
    }
}
