using Application.Common.Dtos.Reports.Course;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Reports.CourseCertificate
{

    public class CourseCertificateCommand : IRequest<CourseCertificateResponceDto>
    {
        public string Name { get; set; } = string.Empty;
        public DateTime PassedDate { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
    }
}
