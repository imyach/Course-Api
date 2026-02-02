using Application.Common.Dtos.Courses;
using Application.Common.Dtos.Materials;
using Application.Common.Dtos.ProgressUsers;
using Application.Common.Dtos.Users;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.ProgressMaterials
{
    public class ProgressMaterialLookupDto : IMapWith<ProgressMaterial>
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? StartedAt { get; set; }

        public ProgressModule? ProgressModule { get; set; }
        public UserLookupDto? User { get; set; }
        public MaterialLookupDto? Material { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProgressMaterial, ProgressMaterialLookupDto>()
                .ForMember(progressMaterialVm => progressMaterialVm.Id,
                opt => opt.MapFrom(progressMaterial => progressMaterial.Id))
                .ForMember(progressMaterialVm => progressMaterialVm.Status,
                opt => opt.MapFrom(progressMaterial => progressMaterial.Status))
                .ForMember(progressMaterialVm => progressMaterialVm.StartedAt,
                opt => opt.MapFrom(progressMaterial => progressMaterial.StartedAt))
                .ForMember(progressMaterialVm => progressMaterialVm.ProgressModule,
                opt => opt.MapFrom(progressMaterial => progressMaterial.ProgressModule))
                .ForMember(progressMaterialVm => progressMaterialVm.User,
                opt => opt.MapFrom(progressMaterial => progressMaterial.User))
                .ForMember(progressMaterialVm => progressMaterialVm.Material,
                opt => opt.MapFrom(progressMaterial => progressMaterial.Material));
        }
    }
}
