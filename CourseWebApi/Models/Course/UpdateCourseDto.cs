using Application.Common.Commands.Courses.UpdateCourse;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.Course
{
    public class UpdateCourseDto : IMapWith<UpdateCourseCommand>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        public decimal Rait { get; set; } = 0;
        [Required]
        public Guid UserId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateCourseDto, UpdateCourseCommand>()
                .ForMember(courseVm => courseVm.Id,
                opt => opt.MapFrom(courseCr => courseCr.Id))
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
