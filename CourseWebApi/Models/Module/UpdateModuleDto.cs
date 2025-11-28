using Application.Common.Commands.Modules.CreateModule;
using Application.Common.Commands.Modules.UpdateModule;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.Module
{
    public class UpdateModuleDto : IMapWith<UpdateModuleCommand>
    {
        [Required]
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Order { get; set; }
        public Guid CourseId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateModuleDto, UpdateModuleCommand>()
                .ForMember(moduleVm => moduleVm.Id,
                opt => opt.MapFrom(module => module.Id))
                .ForMember(moduleVm => moduleVm.Title,
                opt => opt.MapFrom(module => module.Title))
                .ForMember(moduleVm => moduleVm.Description,
                opt => opt.MapFrom(module => module.Description))
                .ForMember(moduleVm => moduleVm.Order,
                opt => opt.MapFrom(module => module.Order))
                .ForMember(moduleVm => moduleVm.CourseId,
                opt => opt.MapFrom(module => module.CourseId));

        }
    }
}
