using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Modules.GetModule
{
    public class GetDetailsModuleQueryValidator : AbstractValidator<GetDetailsModuleQuery>
    {
        public GetDetailsModuleQueryValidator() 
        {
            RuleFor(getDetailsModuleQuery => getDetailsModuleQuery.Id)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
