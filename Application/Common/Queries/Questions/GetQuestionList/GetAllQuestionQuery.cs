using Application.Common.Dtos.Questions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Questions.GetQuestionList
{
    public class GetAllQuestionQuery : IRequest<QuestionListVm>
    {
        public Guid TestId { get; set; }
    }
}
