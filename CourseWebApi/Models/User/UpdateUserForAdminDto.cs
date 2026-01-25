using Application.Common.Commands.Users.CreateUser;
using Application.Common.Commands.Users.UpdateUser;
using Application.Common.Commands.Users.UpdateUserForAdmin;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.User
{
    public class UpdateUserForAdminDto : IMapWith<UpdateUserForAdminCommand>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string NameUser { get; set; } = string.Empty;
        [Required]
        public string Login { get; set; } = string.Empty;
        [Required]
        public Role Role { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateUserForAdminDto, UpdateUserForAdminCommand>()
                .ForMember(userCommand => userCommand.Login,
                opt => opt.MapFrom(userDto => userDto.Login))
                .ForMember(userCommand => userCommand.Id,
                opt => opt.MapFrom(userDto => userDto.Id))
                .ForMember(userCommand => userCommand.NameUser,
                opt => opt.MapFrom(userDto => userDto.NameUser))
                .ForMember(userCommand => userCommand.Email,
                opt => opt.MapFrom(userDto => userDto.Email))
                .ForMember(userCommand => userCommand.Role,
                opt => opt.MapFrom(userDto => userDto.Role))
                .ForMember(userCommand => userCommand.PhoneNumber,
                opt => opt.MapFrom(userDto => userDto.PhoneNumber));
        }
    }
}
