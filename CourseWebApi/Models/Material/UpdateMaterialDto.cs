using Application.Common.Commands.Materials.CreateMaterial;
using Application.Common.Commands.Materials.UpdateMaterial;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.Material
{
    public class UpdateMaterialDto : IMapWith<UpdateMaterialCommand>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateMaterialDto, UpdateMaterialCommand>()
                .ForMember(matherialCm => matherialCm.Id,
                opt => opt.MapFrom(matherial => matherial.Id))
                .ForMember(matherialCm => matherialCm.Title,
                opt => opt.MapFrom(matherial => matherial.Title))
                .ForMember(matherialCm => matherialCm.Description,
                opt => opt.MapFrom(matherial => matherial.Description));
        }

    }
}
