using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.AnswersUsers.CreateAnswersUser
{
    public class CreateAnswersUserCommand : IRequest<Guid>
    {
        public Guid CurrentUserId { get; set; }
        public Guid UserId { get; set; }
        public Guid AnswerId { get; set; }

    }
}
