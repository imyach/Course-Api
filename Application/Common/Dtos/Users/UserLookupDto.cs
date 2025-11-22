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
        public string NameUser { get; set; }

        public void Mappings(Profile profile)
        {
            profile.CreateMap<User, UserLooupDto>()
                .ForMember(userVm => userVm.Id,
                 opt => opt.MapFrom(user => user.Id))
                .ForMember(userVm => userVm.NameUser,
                 opt => opt.MapFrom(user => user.NameUser));
        }
    }
}
