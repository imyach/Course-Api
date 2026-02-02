using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.TestResults.CreateTestResults
{
    public class CreateTestResultCommand : IRequest<Guid>
    {
        public Guid CurrentUserId { get; set; }
        public Guid ProgressMaterialId { get; set; }
        public Guid TestId { get; set; }
    }
}
