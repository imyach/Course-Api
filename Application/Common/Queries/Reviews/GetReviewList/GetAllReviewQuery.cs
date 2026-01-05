using Application.Common.Dtos.Reviews;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Reviews.GetReviewList
{
    public class GetAllReviewQuery : IRequest<object[]>
    {
        public Guid IdCourse {  get; set; }
        public int PageSize {  get; set; }
        public int PageNumber {  get; set; }
    }
}
