using Application.Common.Commands.ProgressMaterials.CreateProgressMaterials;
using Application.Common.Commands.ProgressModules.CreateProgressModules;
using AutoMapper;
using CourseWebApi.Models.ProgressMaterial;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.ProgressModule
{
    public class CreateProgressModuleDto : IMapWith<CreateProgressModuleCommand>
    {
        [Required]
        public Guid ProgressUserId { get; set; }
        [Required]
        public Guid ModuleId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateProgressModuleDto, CreateProgressModuleCommand>()
                .ForMember(progressMaterialVm => progressMaterialVm.ProgressUserId,
                opt => opt.MapFrom(progressMaterialDto => progressMaterialDto.ProgressUserId))
                .ForMember(progressMaterialVm => progressMaterialVm.ModuleId,
                opt => opt.MapFrom(progressMaterialDto => progressMaterialDto.ModuleId));
        }
    }
}
