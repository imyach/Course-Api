using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CourseDesktopClient.Models.DtosModel.Auth
{
    public class TokensDto
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; } = string.Empty;
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
