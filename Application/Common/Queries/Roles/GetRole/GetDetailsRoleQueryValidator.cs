using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Roles.GetRole
{
    public class GetAllRoleQueryValidator : AbstractValidator<GetDetailsRoleQuery>
    {
        public GetAllRoleQueryValidator() 
        {
            RuleFor(getDetailsRoleQuery => getDetailsRoleQuery.Id)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(getDetailsRoleQuery => getDetailsRoleQuery.CurrentUserId)
               .NotNull().NotEqual(Guid.Empty);
        }
    }
}
