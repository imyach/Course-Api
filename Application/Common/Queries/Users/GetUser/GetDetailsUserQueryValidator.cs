using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Users.GetUser
{
    public class GetAllUsersQueryValidator : AbstractValidator<GetDetailsUserQuery>
    {
        public GetAllUsersQueryValidator() 
        {
            RuleFor(getDetailsUserQuery => getDetailsUserQuery.Id)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
