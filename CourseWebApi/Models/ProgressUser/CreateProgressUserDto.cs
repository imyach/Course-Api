using Application.Common.Commands.ProgressUsers.CreateProgressUser;
using Application.Common.Commands.Rewies.CreateReview;
using AutoMapper;
using CourseWebApi.Models.Reviews;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.ProgressUser
{
    public class CreateProgressUserDto : IMapWith<CreateProgressUserCommand>
    {
        [Required]
        public Guid CourseId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateProgressUserDto, CreateProgressUserCommand>()
                .ForMember(progressUserCm => progressUserCm.CourseId,
                opt => opt.MapFrom(progressUserDto => progressUserDto.CourseId));
        }
    }
}
