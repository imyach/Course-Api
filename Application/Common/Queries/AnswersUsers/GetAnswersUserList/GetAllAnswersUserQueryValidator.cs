using Application.Common.Queries.AnswersUsers.GetAnswersUserList;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.AnswersUsers.GetAnswersUser
{
    public  class GetAllAnswersUserQueryValidator : AbstractValidator<GetAllAnswersUserQuery>
    {
        public GetAllAnswersUserQueryValidator()
        {
            RuleFor(getDetailsAnswersUserQuery => getDetailsAnswersUserQuery.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
