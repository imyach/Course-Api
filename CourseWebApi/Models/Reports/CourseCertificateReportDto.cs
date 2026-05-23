using Application.Common.Commands.Reports.CourseCertificate;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.Reports
{
    public class CourseCertificateReportDto : IMapWith<CourseCertificateCommand>
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime PassedDate { get; set; }

        [Required]
        public string CourseTitle { get; set; } = string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CourseCertificateReportDto, CourseCertificateCommand>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PassedDate, opt => opt.MapFrom(src => src.PassedDate))
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.CourseTitle));
        }
    }
}