using Application.Common.Dtos.Courses;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Modules
{
    public class ModuleLookupDto : IMapWith<Module>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Order { get; set; }

        public CourseLookupDto? Course { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Module, ModuleLookupDto>()
                .ForMember(moduleVm => moduleVm.Id,
                opt => opt.MapFrom(module => module.Id))
                .ForMember(moduleVm => moduleVm.Title,
                opt => opt.MapFrom(module => module.Title))
                .ForMember(moduleVm => moduleVm.Description,
                opt => opt.MapFrom(module => module.Description))
                .ForMember(moduleVm => moduleVm.Order,
                opt => opt.MapFrom(module => module.Order))
                .ForMember(moduleVm => moduleVm.Course,
                opt => opt.MapFrom(module => module.Course));

        }

    }
}
