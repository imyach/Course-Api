using Application.Common.Dtos.Courses;
using Application.Common.Dtos.Materials;
using Application.Common.Dtos.Modules;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Tests
{
    public class TestLookupDto : IMapWith<Test>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public MaterialLookupDto? Matherial {  get; set; }

        public void Mapping(Profile profile) 
        {
            profile.CreateMap<Test, TestLookupDto>()
                .ForMember(testVm => testVm.Id,
                opt => opt.MapFrom(test => test.Id))
                .ForMember(testVm => testVm.Title,
                opt => opt.MapFrom(test => test.Title))
                .ForMember(testVm => testVm.Description,
                opt => opt.MapFrom(test => test.Description))
                .ForMember(testVm => testVm.Matherial,
                opt => opt.MapFrom(test => test.Material));
        }
    }
}
