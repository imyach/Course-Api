using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Materials.GetMaterial
{
    public class GetDetailsMaterialQueryValidator : AbstractValidator<GetDetailsMaterialQuery>
    {
        public GetDetailsMaterialQueryValidator()
        {
            RuleFor(getDetailsMatherialQuery => getDetailsMatherialQuery.Id)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
