using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Modules.GetModule
{
    public class GetAllModuleQueryValidator : AbstractValidator<GetDetailsModuleQuery>
    {
        public GetAllModuleQueryValidator() 
        {
            RuleFor(getDetailsModuleQuery => getDetailsModuleQuery.Id)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
