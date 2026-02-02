using Application.Common.Commands.Materials.CreateMaterial;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.Material
{
    public class CreateMaterialDto : IMapWith<CreateMaterialCommand>
    {
        [Required]
        public Guid ModuleId { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateMaterialDto, CreateMaterialCommand>()
                .ForMember(matherialCm => matherialCm.ModuleId,
                opt => opt.MapFrom(matherial => matherial.ModuleId))
                .ForMember(matherialCm => matherialCm.Title,
                opt => opt.MapFrom(matherial => matherial.Title))
                .ForMember(matherialCm => matherialCm.Description,
                opt => opt.MapFrom(matherial => matherial.Description));
        }


    }
}
