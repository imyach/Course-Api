using Application.Common.Commands.Tests.CreateTest;
using Application.Common.Commands.Tests.UpdateTest;
using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.Tests
{
    public class UpdateTestDto : IMapWith<UpdateTestCommand>
    {
        [Required]
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateTestDto, UpdateTestCommand>()
                .ForMember(testCm => testCm.Id,
                opt => opt.MapFrom(test => test.Id))
                .ForMember(testCm => testCm.Title,
                opt => opt.MapFrom(test => test.Title))
                .ForMember(testCm => testCm.Description,
                opt => opt.MapFrom(test => test.Description));
        }
    }
}
