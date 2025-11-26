using Application.Common.Dtos.Roles;
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
        public RoleLookupDto? Role { get; set; }
        public string? Email{ get; set; }
        public string HashPassword { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserLooupDto>()
                .ForMember(userVm => userVm.Id,
                 opt => opt.MapFrom(user => user.Id))
                .ForMember(userVm => userVm.NameUser,
                 opt => opt.MapFrom(user => user.NameUser))
                .ForMember(userVm => userVm.Role,
                 opt => opt.MapFrom(user => user.Role))
                .ForMember(userVm => userVm.Email,
                 opt => opt.MapFrom(user => user.Email))
                 .ForMember(userVm => userVm.PhoneNumber,
                 opt => opt.MapFrom(user => user.PhoneNumber))
                 .ForMember(userVm => userVm.HashPassword,
                 opt => opt.MapFrom(user => user.HashPassword))
                 .ForMember(userVm => userVm.Login,
                 opt => opt.MapFrom(user => user.Login))
                 .ForMember(userVm => userVm.CreatedAt,
                 opt => opt.MapFrom(user => user.CreatedAt));
        }
    }
}
