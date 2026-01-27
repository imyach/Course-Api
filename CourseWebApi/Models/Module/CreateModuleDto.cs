using Application.Common.Commands.Modules.CreateModule;
using Application.Common.Dtos.Courses;
using Application.Common.Dtos.Modules;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.Module
{
    public class CreateModuleDto : IMapWith<CreateModuleCommand>
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        public Guid CourseId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateModuleDto, CreateModuleCommand>()
                .ForMember(moduleCm => moduleCm.Title,
                opt => opt.MapFrom(module => module.Title))
                .ForMember(moduleCm => moduleCm.Description,
                opt => opt.MapFrom(module => module.Description))
                .ForMember(moduleCm => moduleCm.CourseId,
                opt => opt.MapFrom(module => module.CourseId));

        }
    }
}
