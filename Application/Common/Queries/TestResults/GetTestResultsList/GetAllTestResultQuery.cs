using Application.Common.Dtos.ProgressModules;
using Application.Common.Dtos.TestResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.TestResults.GetTestResultsList
{
    public class GetAllTestResultQuery : IRequest<TestResultListVm>
    {
        public Guid CurrentUserId { get; set; }
        public Guid ProgressMaterialId { get; set; }
    }
}
