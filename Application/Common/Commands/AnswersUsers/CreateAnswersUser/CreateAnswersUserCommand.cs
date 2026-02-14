using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static Application.Common.Dtos.AnswersUsers.TestResult.CheckingResponsesDto;

namespace Application.Common.Commands.AnswersUsers.CreateAnswersUser
{
    public class CreateAnswersUserCommand : IRequest<CompleteTestResponseDto>
    {
        public Guid CurrentUserId { get; set; }
        public Guid TestResultId { get; set; }
        public List<SelectedAnswerDto> SelectedAnswers { get; set; } = new();
        public DateTime CompletedAt { get; set; }
    }
}
