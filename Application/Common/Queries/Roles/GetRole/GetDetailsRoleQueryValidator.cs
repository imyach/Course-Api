using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Roles.GetRole
{
    public class GetDetailsRoleQueryValidator : AbstractValidator<GetDetailsRoleQuery>
    {
        public GetDetailsRoleQueryValidator() 
        {
            RuleFor(getDetailsRoleQuery => getDetailsRoleQuery.Id)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
