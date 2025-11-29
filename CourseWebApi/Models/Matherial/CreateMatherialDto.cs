using Application.Common.Commands.Matherials.CreateMatherial;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.Matherial
{
    public class CreateMatherialDto : IMapWith<CreateMatherialCommand>
    {
        [Required]
        public Guid IdModule { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        public int Order { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateMatherialDto, CreateMatherialCommand>()
                .ForMember(matherialCm => matherialCm.IdModule,
                opt => opt.MapFrom(matherial => matherial.IdModule))
                .ForMember(matherialCm => matherialCm.Title,
                opt => opt.MapFrom(matherial => matherial.Title))
                .ForMember(matherialCm => matherialCm.Description,
                opt => opt.MapFrom(matherial => matherial.Description))
                .ForMember(matherialCm => matherialCm.Order,
                opt => opt.MapFrom(matherial => matherial.Order));
        }


    }
}
