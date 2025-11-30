using Application.Common.Dtos.Courses;
using Application.Common.Dtos.Users;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.ProgressUsers
{
    public class ProgressUserLookupDto : IMapWith<ProgressUser>
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime StartedAt { get; set; }
        public DateTime? FineshedAt { get; set; }

        public CourseLookupDto? Course { get; set; }
        public UserLookupDto? User { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProgressUser, ProgressUserLookupDto>()
                .ForMember(progressUserVm => progressUserVm.Id,
                opt => opt.MapFrom(progressUser => progressUser.Id))
                .ForMember(progressUserVm => progressUserVm.Status,
                opt => opt.MapFrom(progressUser => progressUser.Status))
                .ForMember(progressUserVm => progressUserVm.StartedAt,
                opt => opt.MapFrom(progressUser => progressUser.StartedAt))
                .ForMember(progressUserVm => progressUserVm.FineshedAt,
                opt => opt.MapFrom(progressUser => progressUser.FineshedAt))
                .ForMember(progressUserVm => progressUserVm.Course,
                opt => opt.MapFrom(progressUser => progressUser.Course))
                .ForMember(progressUserVm => progressUserVm.User,
                opt => opt.MapFrom(progressUser => progressUser.User));
        }
    }
}
