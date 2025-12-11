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

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateCourseDto, CreateCourseCommand>()
                .ForMember(courseCm => courseCm.Title,
                opt => opt.MapFrom(course => course.Title))
                .ForMember(courseCm => courseCm.Description,
                opt => opt.MapFrom(course => course.Description))
                .ForMember(courseCm => courseCm.Rait,
                opt => opt.MapFrom(course => course.Rait));
        }
    }
}
