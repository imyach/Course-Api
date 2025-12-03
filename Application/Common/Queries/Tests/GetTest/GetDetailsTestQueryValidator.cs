using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Tests.GetTest
{
    public class GetDetailsTestQueryValidator : AbstractValidator<GetDetailsTestQuery>
    {
        public GetDetailsTestQueryValidator()
        {
            RuleFor(getDetailsTestQuery => getDetailsTestQuery.Id)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
