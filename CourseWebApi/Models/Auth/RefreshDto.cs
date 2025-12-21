using Application.Common.Commands.Auth.Login;
using Application.Common.Commands.Auth.Refresh;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.Auth
{
    public class RefreshDto : IMapWith<RefreshTokenCommand>
    {
        [Required]
        public Guid RefreshToken { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RefreshDto, RefreshTokenCommand>()
                .ForMember(refreshCm => refreshCm.RefreshToken, opt =>
                opt.MapFrom(refreshDto => refreshDto.RefreshToken));
        }
    }
}
