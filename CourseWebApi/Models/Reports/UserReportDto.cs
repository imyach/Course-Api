using Application.Common.Commands.Questions.UpdateQuestion;
using Application.Common.Commands.Reports.UserReport;
using AutoMapper;
using CourseWebApi.Models.Questions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CourseWebApi.Models.Reports
{
    public class UserReportRequestDto : IMapWith<UserReportCommand>
    {
        [Required]
        public string Name {  get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }   
        public Guid? RoleId {  get; set; }
        [Required]
        public bool IsAllTime { get; set; }
        [Required]
        public string Format {  get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserReportRequestDto, UserReportCommand>()
               .ForMember(updateQuestionCommand => updateQuestionCommand.Name,
               opt => opt.MapFrom(updateQuestionDto => updateQuestionDto.Name))
               .ForMember(updateQuestionCommand => updateQuestionCommand.StartDate,
               opt => opt.MapFrom(updateQuestionDto => updateQuestionDto.StartDate))
               .ForMember(updateQuestionCommand => updateQuestionCommand.EndDate,
               opt => opt.MapFrom(updateQuestionDto => updateQuestionDto.EndDate))
               .ForMember(updateQuestionCommand => updateQuestionCommand.RoleId,
               opt => opt.MapFrom(updateQuestionDto => updateQuestionDto.RoleId))
               .ForMember(updateQuestionCommand => updateQuestionCommand.IsAllTime,
               opt => opt.MapFrom(updateQuestionDto => updateQuestionDto.IsAllTime))
               .ForMember(updateQuestionCommand => updateQuestionCommand.Format,
               opt => opt.MapFrom(updateQuestionDto => updateQuestionDto.Format));
        }
    }
}
