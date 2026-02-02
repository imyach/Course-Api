using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Materials.GetMaterialList
{
    public class GetAllMaterialQueryValidator : AbstractValidator<GetAllMaterialQuery>
    {
        public GetAllMaterialQueryValidator() 
        {
            RuleFor(getAllMatherialQuery => getAllMatherialQuery.ModuleId)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
