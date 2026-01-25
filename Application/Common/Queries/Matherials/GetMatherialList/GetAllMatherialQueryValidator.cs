using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Matherials.GetMatherialList
{
    public class GetAllMatherialQueryValidator : AbstractValidator<GetAllMatherialQuery>
    {
        public GetAllMatherialQueryValidator() 
        {
            RuleFor(getAllMatherialQuery => getAllMatherialQuery.ModuleId)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
