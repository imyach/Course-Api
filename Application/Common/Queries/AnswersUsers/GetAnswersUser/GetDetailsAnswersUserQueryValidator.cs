using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.AnswersUsers.GetAnswersUser
{
    public  class GetDetailsAnswersUserQueryValidator : AbstractValidator<GetDetailsAnswersUserQuery>
    {
        public GetDetailsAnswersUserQueryValidator()
        {
            RuleFor(getDetailsAnswersUserQuery => getDetailsAnswersUserQuery.Id)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
