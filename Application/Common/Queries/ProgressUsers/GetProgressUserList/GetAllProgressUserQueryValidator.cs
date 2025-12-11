using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.ProgressUsers.GetProgressUserList
{
    public class GetAllProgressUserQueryValidator : AbstractValidator<GetAllProgressUserQuery>
    {
        public GetAllProgressUserQueryValidator()
        {
            RuleFor(getAllProgressUserQuery => getAllProgressUserQuery.CurrentUserId)
               .NotNull().NotEqual(Guid.Empty);
        }
    }
}
