using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Questions.UpdateQuestion
{
    public class UpdateQuestionCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid CurrentUserId { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
