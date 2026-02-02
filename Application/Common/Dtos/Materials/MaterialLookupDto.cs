using Application.Common.Dtos.Modules;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Materials
{
    public class MaterialLookupDto : IMapWith<Material>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Order { get; set; }

        public ModuleLookupDto? Module { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Material, MaterialLookupDto>()
                .ForMember(matherialVm => matherialVm.Id,
                opt => opt.MapFrom(matherial => matherial.Id))
                .ForMember(matherialVm => matherialVm.Title,
                opt => opt.MapFrom(matherial => matherial.Title))
                .ForMember(matherialVm => matherialVm.Description,
                opt => opt.MapFrom(matherial => matherial.Description))
                .ForMember(matherialVm => matherialVm.Order,
                opt => opt.MapFrom(matherial => matherial.Order))
                .ForMember(matherialVm => matherialVm.Module,
                opt => opt.MapFrom(matherial => matherial.Module));
        }
    }
}
