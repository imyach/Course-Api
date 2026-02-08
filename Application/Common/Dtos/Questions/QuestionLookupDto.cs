using Application.Common.Dtos.Answers;
using Application.Common.Dtos.Tests;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Questions
{
    public class QuestionLookupDto : IMapWith<Question>
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public TestLookupDto? Test {  get; set; }

        public IList<AnswerSimpleDto>? Answers { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Question, QuestionLookupDto>()
                .ForMember(questionVm => questionVm.Id,
                opt => opt.MapFrom(question => question.Id))
                .ForMember(questionVm => questionVm.Text,
                opt => opt.MapFrom(question => question.Text))
                .ForMember(questionVm => questionVm.Test,
                opt => opt.MapFrom(question => question.Test))
                .ForMember(questionVm => questionVm.Answers,
                opt => opt.MapFrom(question => question.Answers));
        }
    }
}
