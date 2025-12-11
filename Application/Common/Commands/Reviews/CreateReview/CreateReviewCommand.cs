using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Rewies.CreateReview
{
    public class CreateReviewCommand : IRequest<Guid>
    {
        public Guid CurrentUserId { get; set; }

        public Guid CourseId { get; set; }
        public int Rait { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
