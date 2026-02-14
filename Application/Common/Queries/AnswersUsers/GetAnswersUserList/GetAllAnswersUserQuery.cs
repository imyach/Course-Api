using Application.Common.Dtos.Answers;
using Application.Common.Dtos.AnswersUsers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using static Application.Common.Dtos.AnswersUsers.TestResult.CheckingResponsesDto;

namespace Application.Common.Queries.AnswersUsers.GetAnswersUserList
{
    public class GetAllAnswersUserQuery : IRequest<TestHistoryVm>
    {
        public Guid CurrentUserId { get; set; }
        public Guid TestId { get; set; } 
    }
}