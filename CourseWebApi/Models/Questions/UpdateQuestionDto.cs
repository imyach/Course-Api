using Application.Common.Commands.Questions.CreateQuestion;
using Application.Common.Commands.Questions.UpdateQuestion;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.Questions
{
    public class UpdateQuestionDto : IMapWith<UpdateQuestionCommand>
    {
        [Required]
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateQuestionDto, UpdateQuestionCommand>()
                .ForMember(questionCm => questionCm.Text,
                opt => opt.MapFrom(question => question.Text))
                .ForMember(questionCm => questionCm.Id,
                opt => opt.MapFrom(question => question.Id));
        }
    }
}
