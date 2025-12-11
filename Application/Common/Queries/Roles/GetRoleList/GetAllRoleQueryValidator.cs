using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Roles.GetRoleList
{
    public class GetAllRoleQueryValidator : AbstractValidator<GetAllRolesQuery>
    {
        public GetAllRoleQueryValidator() 
        {
            RuleFor(getDetailsRoleQuery => getDetailsRoleQuery.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
