using Application.Common.Dtos.ProgressMaterials;
using Application.Common.Dtos.TestResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.TestResults.GetTestResults
{
    public class GetDetailsTestResultQuery : IRequest<TestResultLookupDto>
    {
        public Guid CurrentUserId { get; set; }
        public Guid Id { get; set; }
    }
}
