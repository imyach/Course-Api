using Application.Common.Commands.Answers.CreateAnswer;
using Application.Common.Commands.Answers.UpdateAnswer;
using AutoMapper;
using Domain.Model;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.Answers
{
    public class UpdateAnswerDto : IMapWith<UpdateAnswerCommand>
    {
        [Required]
        public string Text { get; set; } = string.Empty;
        [Required]
        public Guid Id { get; set; }
        [Required]
        public bool IsCorrect { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateAnswerDto, UpdateAnswerCommand>()
                .ForMember(answerCm => answerCm.Text,
                opt => opt.MapFrom(answer => answer.Text))
                .ForMember(answerCm => answerCm.Id,
                opt => opt.MapFrom(answer => answer.Id))
                .ForMember(answerCm => answerCm.IsCorrect,
                opt => opt.MapFrom(answer => answer.IsCorrect));
        }
    }
}