using Application.Common.Commands.AnswersUsers.CreateAnswersUser;
using Application.Common.Commands.Courses.CreateCourse;
using AutoMapper;
using CourseWebApi.Models.Course;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.AnswersUsers
{
    public class CreateAnswersUserDto : IMapWith<CreateAnswersUserCommand>
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid AnswerId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateAnswersUserDto, CreateAnswersUserCommand>()
                .ForMember(answersUserCm => answersUserCm.UserId,
                opt => opt.MapFrom(answersUser => answersUser.UserId))
                .ForMember(answersUserCm => answersUserCm.AnswerId,
                opt => opt.MapFrom(answersUser => answersUser.AnswerId));
        }
    }
}
