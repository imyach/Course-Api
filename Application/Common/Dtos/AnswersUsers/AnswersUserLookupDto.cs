using Application.Common.Dtos.Answers;
using Application.Common.Dtos.Courses;
using Application.Common.Dtos.Questions;
using Application.Common.Dtos.TestResults;
using Application.Common.Dtos.Users;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.AnswersUsers
{
    public class AnswersUserLookupDto : IMapWith<AnswersUser>
    {
        public Guid Id { get; set; }    

        public UserLookupDto? User { get; set; }
        public AnswerLookupDto? Answer { get; set; }
        public QuestionLookupDto? Question { get; set; }
        public TestResultLookupDto? TestResult { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AnswersUser, AnswersUserLookupDto>()
                .ForMember(courseVm => courseVm.Id,
                opt => opt.MapFrom(course => course.Id))
                .ForMember(courseVm => courseVm.User,
                opt => opt.MapFrom(course => course.User))
                .ForMember(courseVm => courseVm.Answer,
                opt => opt.MapFrom(course => course.Answer))
                .ForMember(courseVm => courseVm.Question,
                opt => opt.MapFrom(course => course.Question))
                .ForMember(courseVm => courseVm.TestResult,
                opt => opt.MapFrom(course => course.TestResult));
        }
    }
}
