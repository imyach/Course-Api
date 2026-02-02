using Application.Common.Queries.ProgressUsers.GetProgressUser;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.ProgressModules.GetProgressModules
{
    public class GetDetailsProgressModuleQueryValidator : AbstractValidator<GetDetailsProgressModuleQuery>
    {
        public GetDetailsProgressModuleQueryValidator()
        {
            RuleFor(getDetailsProgressUserQuery => getDetailsProgressUserQuery.Id)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(getDetailsProgressUserQuery => getDetailsProgressUserQuery.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
