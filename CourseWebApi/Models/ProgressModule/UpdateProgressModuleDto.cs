using Application.Common.Commands.ProgressMaterials.UpdateProgressMaterials;
using Application.Common.Commands.ProgressModules.UpdateProgressModules;
using AutoMapper;
using CourseWebApi.Models.ProgressMaterial;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.ProgressModule
{
    public class UpdateProgressModuleDto : IMapWith<UpdateProgressModuleCommand>
    {
        [Required]
        public Guid Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime StartedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateProgressModuleDto, UpdateProgressModuleCommand>()
                .ForMember(progressUserCm => progressUserCm.Id,
                opt => opt.MapFrom(progressUserDto => progressUserDto.Id))
                .ForMember(progressUserCm => progressUserCm.Status,
                opt => opt.MapFrom(progressUserDto => progressUserDto.Status))
                .ForMember(progressUserCm => progressUserCm.StartedAt,
                opt => opt.MapFrom(progressUserDto => progressUserDto.StartedAt));
        }
    }
}
