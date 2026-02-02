using Application.Common.Queries.ProgressMaterials.GetProgressMaterials;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.TestResults.GetTestResults
{
    public class GetDetailsTestResultQueryValidator : AbstractValidator<GetDetailsTestResultQuery>
    {
        public GetDetailsTestResultQueryValidator()
        {
            RuleFor(getDetailsProgressUserQuery => getDetailsProgressUserQuery.Id)
                .NotNull().NotEqual(Guid.Empty);
            RuleFor(getDetailsProgressUserQuery => getDetailsProgressUserQuery.CurrentUserId)
                .NotNull().NotEqual(Guid.Empty);
        }
    }
}
