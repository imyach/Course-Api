using Application.Common.Commands.Matherials.CreateMatherial;
using Application.Common.Commands.Matherials.UpdateMatherial;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.Matherial
{
    public class UpdateMatherialDto : IMapWith<UpdateMatherialCommand>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid ModuleId { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        public int Order { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateMatherialDto, UpdateMatherialCommand>()
                .ForMember(matherialCm => matherialCm.Id,
                opt => opt.MapFrom(matherial => matherial.Id))
                .ForMember(matherialCm => matherialCm.ModuleId,
                opt => opt.MapFrom(matherial => matherial.ModuleId))
                .ForMember(matherialCm => matherialCm.Title,
                opt => opt.MapFrom(matherial => matherial.Title))
                .ForMember(matherialCm => matherialCm.Description,
                opt => opt.MapFrom(matherial => matherial.Description))
                .ForMember(matherialCm => matherialCm.Order,
                opt => opt.MapFrom(matherial => matherial.Order));
        }

    }
}
