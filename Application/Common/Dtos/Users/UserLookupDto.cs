using Application.Common.Mappings;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Users
{
    public class UserLooupDto : IMapWith<User>
    {
        public Guid Id { get; set; }
        public string NameUser { get; set; } = string.Empty;
        public string RoleId{ get; set; } = string.Empty;
        public string? Email{ get; set; }
        public string? PhoneNumber { get; set; }


        public void Mappings(Profile profile)
        {
            profile.CreateMap<User, UserLooupDto>()
                .ForMember(userVm => userVm.Id,
                 opt => opt.MapFrom(user => user.Id))
                .ForMember(userVm => userVm.NameUser,
                 opt => opt.MapFrom(user => user.NameUser))
                .ForMember(userVm => userVm.RoleId,
                 opt => opt.MapFrom(user => user.Role.Id))
                .ForMember(userVm => userVm.Email,
                 opt => opt.MapFrom(user => user.Email))
                 .ForMember(userVm => userVm.PhoneNumber,
                 opt => opt.MapFrom(user => user.PhoneNumber));
        }
    }
}
