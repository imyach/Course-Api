using Application.Common.Queries.ProgressModules.GetProgressModulesList;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.TestResults.GetTestResultsList
{
    public class GetAllTestResultQueryValidator : AbstractValidator<GetAllTestResultQuery>
    {
        public GetAllTestResultQueryValidator()
        {
            RuleFor(getAllProgressUserQuery => getAllProgressUserQuery.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(getAllProgressUserQuery => getAllProgressUserQuery.ProgressMaterialId)
               .NotNull().NotEqual(Guid.Empty);
        }
    }
}
