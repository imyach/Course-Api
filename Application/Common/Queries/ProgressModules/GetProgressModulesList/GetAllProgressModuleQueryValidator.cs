using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.ProgressModules.GetProgressModulesList
{
    public class GetAllProgressModuleQueryValidator : AbstractValidator<GetAllProgressModuleQuery>
    {
        public GetAllProgressModuleQueryValidator()
        {
            RuleFor(getAllProgressUserQuery => getAllProgressUserQuery.CurrentUserId)
              .NotNull().NotEqual(Guid.Empty);
            RuleFor(getAllProgressUserQuery => getAllProgressUserQuery.UserId)
               .NotNull().NotEqual(Guid.Empty);
            RuleFor(getAllProgressUserQuery => getAllProgressUserQuery.ProgressUserId)
               .NotNull().NotEqual(Guid.Empty);
        }
    }
}
