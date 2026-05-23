using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CourseDesktopClient.Models.DtosModel.Reports.Course
{
    public class CourseCertificateResultDto
    {
        [JsonPropertyName("fileContent")]
        public byte[] FileContent { get; set; }
        [JsonPropertyName("fileName")]
        public string FileName { get; set; }
    }
}
