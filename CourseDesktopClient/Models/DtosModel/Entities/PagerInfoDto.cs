using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CourseDesktopClient.Models.DtosModel.Entities
{
    public class PagerInfoDto
    {
        [JsonPropertyName("totalItems")]
        public int TotalItems { get; set; }
        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }
        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; }
    }
}
