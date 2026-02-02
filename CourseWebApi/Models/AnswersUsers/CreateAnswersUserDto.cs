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
        public Guid AnswerId { get; set; }
        [Required]
        public Guid QuestionId { get; set; }
        [Required]
        public Guid TestResultId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateAnswersUserDto, CreateAnswersUserCommand>()
                .ForMember(answersUserCm => answersUserCm.AnswerId,
                opt => opt.MapFrom(answersUser => answersUser.AnswerId))
                .ForMember(answersUserCm => answersUserCm.QuestionId,
                opt => opt.MapFrom(answersUser => answersUser.QuestionId))
                .ForMember(answersUserCm => answersUserCm.TestResultId,
                opt => opt.MapFrom(answersUser => answersUser.TestResultId));
        }
    }
}
