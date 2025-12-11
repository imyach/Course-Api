using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.ProgressUsers.GetProgressUser
{
    public class GetDetailsProgressUserQueryValidator : AbstractValidator<GetDetailsProgressUserQuery>
    {
        public GetDetailsProgressUserQueryValidator()
        {
            RuleFor(getDetailsProgressUserQuery => getDetailsProgressUserQuery.Id)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(getDetailsProgressUserQuery => getDetailsProgressUserQuery.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
