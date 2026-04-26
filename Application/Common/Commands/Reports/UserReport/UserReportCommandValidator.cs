using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Reports.UserReport
{
    public class UserReportCommandValidator: AbstractValidator<UserReportCommand>
    {
        public UserReportCommandValidator()
        {

        }
    }
}
