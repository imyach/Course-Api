using Application.Common.Commands.Answers.CreateAnswer;
using Application.Common.Commands.AnswersUsers.CreateAnswersUser;
using AutoMapper;
using CourseWebApi.Models.AnswersUsers;
using Domain.Model;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.Answers
{
    public class CreateAnswerDto : IMapWith<CreateAnswerCommand>
    {
        [Required]
        public string Text{ get; set; } = string.Empty;
        [Required]
        public Guid QuestionId { get; set; }
        [Required]
        public bool IsCorrect { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateAnswerDto, CreateAnswerCommand>()
                .ForMember(answerCm => answerCm.Text,
                opt => opt.MapFrom(answer => answer.Text))
                .ForMember(answerCm => answerCm.IsCorrect,
                opt => opt.MapFrom(answer => answer.IsCorrect))
                .ForMember(answerCm => answerCm.QuestionId,
                opt => opt.MapFrom(answer => answer.QuestionId));
        }
    }
}