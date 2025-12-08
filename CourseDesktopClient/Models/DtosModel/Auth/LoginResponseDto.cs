using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CourseDesktopClient.Models.DtosModel.Auth
{
    public class LoginResponseDto
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
    }
}
