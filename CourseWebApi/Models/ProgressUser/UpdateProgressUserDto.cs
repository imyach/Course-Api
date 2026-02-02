using Application.Common.Commands.ProgressUsers.CreateProgressUser;
using Application.Common.Commands.ProgressUsers.UpdateProgressUser;
using Application.Common.Commands.Rewies.CreateReview;
using AutoMapper;
using CourseWebApi.Models.Reviews;
using Domain.Model;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.ProgressUser
{
    public class UpdateProgressUserDto : IMapWith<UpdateProgressUserCommand>
    {
        [Required]
        public Guid Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime FinishedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateProgressUserDto, UpdateProgressUserCommand>()
                .ForMember(progressUserCm => progressUserCm.Id,
                opt => opt.MapFrom(progressUserDto => progressUserDto.Id))
                .ForMember(progressUserCm => progressUserCm.Status,
                opt => opt.MapFrom(progressUserDto => progressUserDto.Status))
                .ForMember(progressUserCm => progressUserCm.FinishedAt,
                opt => opt.MapFrom(progressUserDto => progressUserDto.FinishedAt));
        }
    }
}
