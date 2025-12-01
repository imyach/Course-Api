using Application.Common.Dtos.Questions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Questions.GetQuestions
{
    public class GetDetailsQuestionQuery : IRequest<QuestionLookupDto>
    {
        public Guid Id { get; set; }
        public Guid CurrentUserId { get; set; }

    }
}
