using Application.Common.Mappings;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Answers
{
    public class AnswerSimpleDto : IMapWith<Answer>
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Answer, AnswerSimpleDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect));
        }
    }
}
