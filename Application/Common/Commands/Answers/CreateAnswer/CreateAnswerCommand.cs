using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.Answers.CreateAnswer
{
    public class CreateAnswerCommand : IRequest<Guid>
    {
        public Guid CurrentUserId { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public Guid QuestionId { get; set; }

    }
}
