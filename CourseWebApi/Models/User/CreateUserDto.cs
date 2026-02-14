using Application.Common.Commands.Users.CreateUser;
using AutoMapper;
using Domain.Model;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.User
{
    public class CreateUserDto: IMapWith<CreateUserCommand>
    {
        [Required]
        public string NameUser { get; set; } = string.Empty;
        [Required]
        public string Login { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public Role? Role { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateUserDto, CreateUserCommand>()
                .ForMember(userCm => userCm.Login,
                opt => opt.MapFrom(userDto => userDto.Login))
                .ForMember(userCm => userCm.NameUser,
                opt => opt.MapFrom(userDto => userDto.NameUser))
                .ForMember(userCm => userCm.Email,
                opt => opt.MapFrom(userDto => userDto.Email))
                .ForMember(userCm => userCm.Password,
                opt => opt.MapFrom(userDto => userDto.Password))
                .ForMember(userCm => userCm.Role,
                opt => opt.MapFrom(userDto => userDto.Role))
                .ForMember(userCm => userCm.PhoneNumber,
                opt => opt.MapFrom(userDto => userDto.PhoneNumber));
        }
    }
}
