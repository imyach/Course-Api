using Application.Common.Dtos.Courses;
using Application.Common.Dtos.Questions;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Answers
{
    public class AnswerLookupDto : IMapWith<Answer>
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public QuestionLookupDto? Question { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Answer, AnswerLookupDto>()
                .ForMember(courseVm => courseVm.Id,
                opt => opt.MapFrom(course => course.Id))
                .ForMember(courseVm => courseVm.Text,
                opt => opt.MapFrom(course => course.Text))
                .ForMember(courseVm => courseVm.IsCorrect,
                opt => opt.MapFrom(course => course.IsCorrect))
                .ForMember(courseVm => courseVm.Question,
                opt => opt.MapFrom(course => course.Question));
        }
    }
}
