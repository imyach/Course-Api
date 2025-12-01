using Application.Common.Commands.Questions.CreateQuestion;
using AutoMapper;
using Domain.Model;

namespace CourseWebApi.Models.Questions
{
    public class CreateQuestionDto : IMapWith<CreateQuestionCommand>
    {
        public Guid TestId { get; set; }
        public string Text { get; set; } = string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateQuestionDto, CreateQuestionCommand>()
                .ForMember(questionCm => questionCm.TestId,
                opt => opt.MapFrom(question => question.TestId))
                .ForMember(questionCm => questionCm.Text,
                opt => opt.MapFrom(question => question.Text));
        }
    }
}
