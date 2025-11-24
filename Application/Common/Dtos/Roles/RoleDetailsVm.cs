using Application.Common.Mappings;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Roles
{
    public class RoleDetailsVm :IMapWith<Role>
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Role, RoleDetailsVm>()
                .ForMember(roleVm => roleVm.Id,
                opt => opt.MapFrom(role => role.Id))
                .ForMember(roleVm => roleVm.RoleName,
                opt => opt.MapFrom(role => role.RoleName))
                .ForMember(roleVm => roleVm.CreatedAt,
                opt => opt.MapFrom(role => role.CreatedAt));
        }
    }
}
