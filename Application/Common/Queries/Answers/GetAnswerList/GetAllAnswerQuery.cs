using Application.Common.Dtos.Answers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Answers.GetAnswerList
{
    public  class GetAllAnswerQuery : IRequest<AnswerListVm>
    {
    }
}
