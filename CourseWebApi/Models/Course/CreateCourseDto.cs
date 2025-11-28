using Application.Common.Commands.Courses.CreateCourse;
using AutoMapper;
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
                .ForMember(courseVm => courseVm.Title,
                opt => opt.MapFrom(courseCr => courseCr.Title))
                .ForMember(courseVm => courseVm.Description,
                opt => opt.MapFrom(courseCr => courseCr.Description))
                .ForMember(courseVm => courseVm.Rait,
                opt => opt.MapFrom(courseCr => courseCr.Rait))
                .ForMember(courseVm => courseVm.UserId,
                opt => opt.MapFrom(courseCr => courseCr.UserId));
        }
    }
}
