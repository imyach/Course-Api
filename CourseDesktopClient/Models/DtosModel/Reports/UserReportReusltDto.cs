using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CourseDesktopClient.Models.DtosModel.Reports
{
    public class UserReportReusltDto
    {
        [JsonPropertyName("fileContent")]
        public byte[] FileContent { get; set; }
        [JsonPropertyName("fileName")]
        public string FileName { get; set; }
        [JsonPropertyName("contentType")]
        public string ContentType { get; set; }
    }
}
