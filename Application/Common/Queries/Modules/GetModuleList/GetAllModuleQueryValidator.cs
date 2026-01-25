using Application.Common.Queries.Matherials.GetMatherialList;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Modules.GetModuleList
{
    public class GetAllModuleQueryValidator : AbstractValidator<GetAllModuleQuery>
    {
        public GetAllModuleQueryValidator() 
        {
            RuleFor(getAllModuleQuery => getAllModuleQuery.CourseId)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
