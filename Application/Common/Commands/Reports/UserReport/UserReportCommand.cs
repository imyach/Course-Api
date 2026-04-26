using Application.Common.Dtos.Reports.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Reports.UserReport
{
    public class UserReportCommand : IRequest<UserReportReusltDto>
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid? RoleId { get; set; }
        public bool IsAllTime { get; set; }
        public string Format { get; set; }
    }
}
