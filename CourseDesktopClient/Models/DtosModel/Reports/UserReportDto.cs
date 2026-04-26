using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.Models.DtosModel.Reports
{
    public class UserReportDto
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }   
        public Guid? RoleId {  get; set; }
        public bool IsAllTime { get; set; }
        public string Format {  get; set; }
    }
}
