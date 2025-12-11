using Application.Common.Dtos.Reviews;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Reviews.GetReview
{
    public class GetDetailsReviewQuery : IRequest<ReviewLookupDto>
    {
        public Guid Id { get; set; }
    }
}
