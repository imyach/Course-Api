using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.TestResults.UpdateTestResults
{
    public class UpdateTestResultCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid CurrentUserId { get; set; }

        public int Score { get; set; }
        public bool IsPassed { get; set; }
        public DateTime? ComplitedAt { get; set; }
    }
}
