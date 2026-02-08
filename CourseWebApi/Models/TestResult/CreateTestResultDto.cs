using Application.Common.Commands.TestResults.CreateTestResults;
using AutoMapper;
using CourseWebApi.Models.ProgressMaterial;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.TestResult
{
    public class CreateTestResultDto : IMapWith<CreateTestResultCommand>
    {
        [Required]
        public Guid ProgressMaterialId { get; set; }
        [Required]
        public Guid TestId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateTestResultDto, CreateTestResultCommand>()
                .ForMember(progressMaterialVm => progressMaterialVm.ProgressMaterialId,
                opt => opt.MapFrom(progressMaterialDto => progressMaterialDto.ProgressMaterialId))
                .ForMember(progressMaterialVm => progressMaterialVm.TestId,
                opt => opt.MapFrom(progressMaterialDto => progressMaterialDto.TestId));
        }
    }
}
