using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Common.Commands.AnswersUsers.CreateAnswersUser
{
    public class CreateAnswersUserCommand : IRequest<Guid>
    {
        public Guid CurrentUserId { get; set; }
        public Guid AnswerId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid TestResultId { get; set; }

    }
}
