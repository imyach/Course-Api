using Application.Common.Dtos.Reviews;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Reviews.GetReviewList
{
    public class GetAllReviewQuery : IRequest<ReviewListVm>
    {
        public Guid CurrentUserId {  get; set; }
    }
}
