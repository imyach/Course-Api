using Application.Common.Dtos.Users;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Courses
{
    public class CourseLookupDto : IMapWith<Course>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public decimal? Rait { get; set; }

        public UserLookupDto? User { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Course, CourseLookupDto>()
                .ForMember(courseVm => courseVm.Id,
                opt => opt.MapFrom(course => course.Id))
                .ForMember(courseVm => courseVm.Title,
                opt => opt.MapFrom(course => course.Title))
                .ForMember(courseVm => courseVm.Description,
                opt => opt.MapFrom(course => course.Description))
                .ForMember(courseVm => courseVm.Status,
                opt => opt.MapFrom(course => course.Status))
                .ForMember(courseVm => courseVm.CreatedAt,
                opt => opt.MapFrom(course => course.CreatedAt))
                .ForMember(courseVm => courseVm.UpdateAt,
                opt => opt.MapFrom(course => course.UpdateAt))
                .ForMember(courseVm => courseVm.Rait,
                opt => opt.MapFrom(course => course.Rait))
                .ForMember(courseVm => courseVm.User,
                opt => opt.MapFrom(course => course.User));

        }
    }
}
