using Application.Common.Commands.Users.CreateUser;
using Application.Common.Commands.Users.UpdateUser;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.User
{
    public class UpdateUserDto : IMapWith<UpdateUserCommand>
    {
        [Required]
        public Guid Id { get; set; }
        public string NameUser { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string HashPassword { get; set; } = string.Empty;
        [Required]
        public Guid RoleId { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateUserDto, UpdateUserCommand>()
                .ForMember(userCommand => userCommand.Login,
                opt => opt.MapFrom(userDto => userDto.Login))
                .ForMember(userCommand => userCommand.Id,
                opt => opt.MapFrom(userDto => userDto.Id))
                .ForMember(userCommand => userCommand.NameUser,
                opt => opt.MapFrom(userDto => userDto.NameUser))
                .ForMember(userCommand => userCommand.Email,
                opt => opt.MapFrom(userDto => userDto.Email))
                .ForMember(userCommand => userCommand.HashPassword,
                opt => opt.MapFrom(userDto => userDto.HashPassword))
                .ForMember(userCommand => userCommand.RoleId,
                opt => opt.MapFrom(userDto => userDto.RoleId))
                .ForMember(userCommand => userCommand.PhoneNumber,
                opt => opt.MapFrom(userDto => userDto.PhoneNumber));
        }
    }
}
