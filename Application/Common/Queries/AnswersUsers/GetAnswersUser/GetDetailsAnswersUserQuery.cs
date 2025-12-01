using Application.Common.Dtos.Answers;
using Application.Common.Dtos.AnswersUsers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.AnswersUsers.GetAnswersUser
{
    public class GetDetailsAnswersUserQuery : IRequest<AnswersUserLookupDto>
    {
        public Guid Id { get; set; }
        public Guid CurrentUserId { get; set; }
    }
}
