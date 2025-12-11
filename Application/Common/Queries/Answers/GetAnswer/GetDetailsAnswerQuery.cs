using Application.Common.Dtos.Answers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Answers.GetAnswer
{
    public class GetDetailsAnswerQuery : IRequest<AnswerLookupDto>
    {
        public Guid Id { get; set; }
    }
}
