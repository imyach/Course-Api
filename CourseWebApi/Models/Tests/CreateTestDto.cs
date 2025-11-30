using Application.Common.Commands.Tests.CreateTest;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.Tests
{
    public class CreateTestDto : IMapWith<CreateTestCommand>
    {
        public Guid MatherialId { get; set; }
        public Guid CourseId { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateTestDto, CreateTestCommand>()
                .ForMember(testCm => testCm.MatherialId,
                opt => opt.MapFrom(test => test.MatherialId))
                .ForMember(testCm => testCm.CourseId,
                opt => opt.MapFrom(test => test.CourseId))
                .ForMember(testCm => testCm.Title,
                opt => opt.MapFrom(test => test.Title))
                .ForMember(testCm => testCm.Description,
                opt => opt.MapFrom(test => test.Description));
        }
    }
}
