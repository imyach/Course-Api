using Application.Common.Commands.ProgressMaterials.UpdateProgressMaterials;
using Application.Common.Commands.ProgressUsers.UpdateProgressUser;
using AutoMapper;
using CourseWebApi.Models.ProgressUser;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.ProgressMaterial
{
    public class UpdateProgressMaterialDto : IMapWith<UpdateProgressMaterialCommand>
    {
        [Required]
        public Guid Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime StartedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateProgressMaterialDto, UpdateProgressMaterialCommand>()
                .ForMember(progressUserCm => progressUserCm.Id,
                opt => opt.MapFrom(progressUserDto => progressUserDto.Id))
                .ForMember(progressUserCm => progressUserCm.Status,
                opt => opt.MapFrom(progressUserDto => progressUserDto.Status))
                .ForMember(progressUserCm => progressUserCm.StartedAt,
                opt => opt.MapFrom(progressUserDto => progressUserDto.StartedAt));
        }
    }
}
