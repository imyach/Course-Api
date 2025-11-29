using Application.Common.Commands.Courses.CreateCourse;
using AutoMapper;
using Domain.Model;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.Course
{
    public class CreateCourseDto : IMapWith<CreateCourseCommand>
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Rait { get; set; } = 0;
        [Required]
        public Guid UserId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateCourseDto, CreateCourseCommand>()
                .ForMember(courseCm => courseCm.Title,
                opt => opt.MapFrom(courseCr => courseCr.Title))
                .ForMember(courseCm => courseCm.Description,
                opt => opt.MapFrom(courseCr => courseCr.Description))
                .ForMember(courseCm => courseCm.Rait,
                opt => opt.MapFrom(courseCr => courseCr.Rait))
                .ForMember(courseCm => courseCm.UserId,
                opt => opt.MapFrom(courseCr => courseCr.UserId));
        }
    }
}
