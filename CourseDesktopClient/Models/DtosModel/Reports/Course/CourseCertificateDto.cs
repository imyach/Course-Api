using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CourseDesktopClient.Models.DtosModel.Reports.Course
{
    public class CourseCertificateDto
    {
        public string Name { get; set; }
        public DateTime? PassedDate { get; set; }
        public string CourseTitle { get; set; }

    }
}
