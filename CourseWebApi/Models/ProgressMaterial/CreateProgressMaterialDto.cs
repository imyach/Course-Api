using Application.Common.Commands.ProgressMaterials.CreateProgressMaterials;
using Application.Common.Commands.ProgressUsers.CreateProgressUser;
using AutoMapper;
using CourseWebApi.Models.ProgressUser;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.ProgressMaterial
{
    public class CreateProgressMaterialDto : IMapWith<CreateProgressMaterialCommand>
    {
        [Required]
        public Guid ProgressModuleId { get; set; }
        [Required]
        public Guid MaterialId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateProgressMaterialDto, CreateProgressMaterialCommand>()
                .ForMember(progressMaterialVm => progressMaterialVm.ProgressModuleId,
                opt => opt.MapFrom(progressMaterialDto => progressMaterialDto.ProgressModuleId))
                .ForMember(progressMaterialVm => progressMaterialVm.MaterialId,
                opt => opt.MapFrom(progressMaterialDto => progressMaterialDto.MaterialId));
        }
    }
}
