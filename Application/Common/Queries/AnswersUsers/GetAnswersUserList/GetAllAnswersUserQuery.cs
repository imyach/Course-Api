using Application.Common.Dtos.Answers;
using Application.Common.Dtos.AnswersUsers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.AnswersUsers.GetAnswersUserList
{
    public class GetAllAnswersUserQuery : IRequest<AnswersUserListVm>
    {
        public Guid CurrentUserId { get; set; }
        public Guid TestResultsId { get; set; }
    }
}
