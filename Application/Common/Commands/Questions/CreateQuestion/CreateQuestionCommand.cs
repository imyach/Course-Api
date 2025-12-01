using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Questions.CreateQuestion
{
    public class CreateQuestionCommand : IRequest<Guid>
    {
        public Guid CurrentUserId { get; set; }

        public Guid TestId { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
