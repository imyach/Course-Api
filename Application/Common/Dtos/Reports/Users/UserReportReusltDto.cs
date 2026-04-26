using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Reports.Users
{
    public class UserReportReusltDto
    {
        public byte[] FileContent { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
