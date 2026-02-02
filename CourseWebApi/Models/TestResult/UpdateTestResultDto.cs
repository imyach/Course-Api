using Application.Common.Commands.ProgressMaterials.UpdateProgressMaterials;
using Application.Common.Commands.TestResults.UpdateTestResults;
using AutoMapper;
using CourseWebApi.Models.ProgressMaterial;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.TestResult
{
    public class UpdateTestResultDto : IMapWith<UpdateTestResultCommand>
    {
        [Required]
        public Guid Id { get; set; }
        public int Score { get; set; }
        public bool IsPassed { get; set; }
        public DateTime? ComplitedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateTestResultDto, UpdateTestResultCommand>()
                .ForMember(progressUserCm => progressUserCm.Id,
                opt => opt.MapFrom(progressUserDto => progressUserDto.Id))
                .ForMember(progressUserCm => progressUserCm.Score,
                opt => opt.MapFrom(progressUserDto => progressUserDto.Score))
                .ForMember(progressUserCm => progressUserCm.ComplitedAt,
                opt => opt.MapFrom(progressUserDto => progressUserDto.ComplitedAt))
                .ForMember(progressUserCm => progressUserCm.IsPassed,
                opt => opt.MapFrom(progressUserDto => progressUserDto.IsPassed));
        }
    }
}
