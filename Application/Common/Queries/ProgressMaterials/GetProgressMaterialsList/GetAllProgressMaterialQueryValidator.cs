using Application.Common.Queries.ProgressModules.GetProgressModulesList;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.ProgressMaterials.GetProgressMaterialsList
{
    public class GetAllProgressMaterialQueryValidator : AbstractValidator<GetAllProgressMaterialQuery>
    {
        public GetAllProgressMaterialQueryValidator()
        {
            RuleFor(getAllProgressUserQuery => getAllProgressUserQuery.CurrentUserId)
              .NotNull().NotEqual(Guid.Empty);
            RuleFor(getAllProgressUserQuery => getAllProgressUserQuery.UserId)
               .NotNull().NotEqual(Guid.Empty);
            RuleFor(getAllProgressUserQuery => getAllProgressUserQuery.ProgressModuleId)
               .NotNull().NotEqual(Guid.Empty);
        }
    }
}
