using Application.Common.Commands.Auth.Login;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.Auth
{
    public class LoginDto : IMapWith<LoginUserCommand>
    {
        [Required]
        public string Login { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

        public void Mapping(Profile profile) 
        {
            profile.CreateMap<LoginDto, LoginUserCommand>()
                .ForMember(loginCm => loginCm.Login, opt =>
                opt.MapFrom(loginDto => loginDto.Login))
                .ForMember(loginCm => loginCm.Password, opt =>
                opt.MapFrom(loginDto => loginDto.Password));
        }


    }
}
