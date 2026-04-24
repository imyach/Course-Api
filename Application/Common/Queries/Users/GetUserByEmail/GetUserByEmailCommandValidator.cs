using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Users.GetUserByEmail
{
    public class GetUserByEmailCommandValidator : AbstractValidator<GetUserByEmailCommand>
    {
        public GetUserByEmailCommandValidator()
        {

            RuleFor(getUserByEmailCommand => getUserByEmailCommand.Email)
               .NotNull().NotEmpty();
        }
    }
}
