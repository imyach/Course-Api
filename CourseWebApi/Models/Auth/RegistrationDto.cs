using Application.Common.Commands.Auth.Registration;
using Application.Common.Commands.Users.CreateUser;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.Auth
{
    public class RegistrationDto : IMapWith<RegistrationUserCommand>
    {
        [Required]
        public string NameUser { get; set; } = string.Empty;
        [Required]
        public string Login { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RegistrationDto, RegistrationUserCommand>()
                .ForMember(userCm => userCm.Login,
                opt => opt.MapFrom(userDto => userDto.Login))
                .ForMember(userCm => userCm.NameUser,
                opt => opt.MapFrom(userDto => userDto.NameUser))
                .ForMember(userCm => userCm.Email,
                opt => opt.MapFrom(userDto => userDto.Email))
                .ForMember(userCm => userCm.Password,
                opt => opt.MapFrom(userDto => userDto.Password))
                .ForMember(userCm => userCm.PhoneNumber,
                opt => opt.MapFrom(userDto => userDto.PhoneNumber));
        }
    }
}
