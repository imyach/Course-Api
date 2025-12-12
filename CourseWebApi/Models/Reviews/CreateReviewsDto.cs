using Application.Common.Commands.Rewies.CreateReview;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace CourseWebApi.Models.Reviews
{
    public class CreateReviewsDto : IMapWith<CreateReviewCommand>
    {
        [Required]
        public Guid CourseId { get; set; }
        [Required]
        public int Rait {  get; set; }
        [Required]
        public string Text { get; set; } = string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateReviewsDto, CreateReviewCommand>()
                .ForMember(reviewCm => reviewCm.Text,
                opt => opt.MapFrom(reviewDto => reviewDto.Text))
                .ForMember(reviewCm => reviewCm.Rait,
                opt => opt.MapFrom(reviewDto => reviewDto.Rait))
                .ForMember(reviewCm => reviewCm.CourseId,
                opt => opt.MapFrom(reviewDto => reviewDto.CourseId));
        }

    }
}
