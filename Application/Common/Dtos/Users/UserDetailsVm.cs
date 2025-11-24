using Application.Common.Mappings;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Users
{
    public class UserDetailsVm : IMapWith<User>
    {
        public Guid Id { get; set; }
        public string NameUser { get; set; }
        public string RoleId { get; set; }
        public string HashPassword { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? PhoneNumber { get; set; }

        public void Mappings(Profile profile)
        {
            profile.CreateMap<User, UserDetailsVm>()
                .ForMember(userVm => userVm.Id,
                 opt => opt.MapFrom(user => user.Id))
                .ForMember(userVm => userVm.NameUser,
                 opt => opt.MapFrom(user => user.NameUser))
                .ForMember(userVm => userVm.RoleId,
                 opt => opt.MapFrom(user => user.RoleId))
                .ForMember(userVm => userVm.HashPassword,
                 opt => opt.MapFrom(user => user.HashPassword))
                .ForMember(userVm => userVm.Email,
                 opt => opt.MapFrom(user => user.Email))
                .ForMember(userVm => userVm.CreatedAt,
                 opt => opt.MapFrom(user => user.CreatedAt))
                .ForMember(userVm => userVm.PhoneNumber,
                 opt => opt.MapFrom(user => user.PhoneNumber));
        }
    }
}
