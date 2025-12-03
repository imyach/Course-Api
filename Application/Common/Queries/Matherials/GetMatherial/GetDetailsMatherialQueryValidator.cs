using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Matherials.GetMatherial
{
    public class GetDetailsMatherialQueryValidator : AbstractValidator<GetDetailsMatherialQuery>
    {
        public GetDetailsMatherialQueryValidator()
        {
            RuleFor(getDetailsMatherialQuery => getDetailsMatherialQuery.Id)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
