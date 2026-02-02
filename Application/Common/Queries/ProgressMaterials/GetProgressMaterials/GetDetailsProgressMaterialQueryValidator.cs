using Application.Common.Queries.ProgressModules.GetProgressModules;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.ProgressMaterials.GetProgressMaterials
{
    public class GetDetailsProgressMaterialQueryValidator : AbstractValidator<GetDetailsProgressMaterialQuery>
    {
        public GetDetailsProgressMaterialQueryValidator()
        {
            RuleFor(getDetailsProgressUserQuery => getDetailsProgressUserQuery.Id)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(getDetailsProgressUserQuery => getDetailsProgressUserQuery.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
