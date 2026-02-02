using Application.Common.Dtos.Modules;
using Application.Common.Dtos.ProgressMaterials;
using Application.Common.Dtos.Users;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Application.Common.Dtos.ProgressModules
{
    public class ProgressModuleLookupDto : IMapWith<ProgressModule>
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? StartedAt { get; set; }

        public ProgressUser? ProgressUser { get; set; }
        public UserLookupDto? User { get; set; }
        public ModuleLookupDto? Module { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProgressModule, ProgressModuleLookupDto>()
                .ForMember(progressModulelVm => progressModulelVm.Id,
                opt => opt.MapFrom(progressModule => progressModule.Id))
                .ForMember(progressModulelVm => progressModulelVm.Status,
                opt => opt.MapFrom(progressModule => progressModule.Status))
                .ForMember(progressModulelVm => progressModulelVm.StartedAt,
                opt => opt.MapFrom(progressModule => progressModule.StartedAt))
                .ForMember(progressModulelVm => progressModulelVm.ProgressUser,
                opt => opt.MapFrom(progressModule => progressModule.ProgressUser))
                .ForMember(progressModulelVm => progressModulelVm.User,
                opt => opt.MapFrom(progressModule => progressModule.User))
                .ForMember(progressModulelVm => progressModulelVm.Module,
                opt => opt.MapFrom(progressModule => progressModule.Module));
        }
    }
}
