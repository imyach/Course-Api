using Application.Common.Dtos.Courses;
using Application.Common.Dtos.Users;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Reviews
{
    public class ReviewLookupDto : IMapWith<Review>
    {
        public Guid Id { get; set; }
        public int Rait {  get; set; }
        public string Text {  get; set; } = string.Empty;
        public DateTime CreatedAt {  get; set; }

        public UserLooupDto? User { get; set; }
        public CourseLookupDto? Course { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Review, ReviewLookupDto>()
                .ForMember(reviewVm => reviewVm.Id,
                opt => opt.MapFrom(review => review.Id))
                .ForMember(reviewVm => reviewVm.Rait,
                opt => opt.MapFrom(review => review.Rait))
                .ForMember(reviewVm => reviewVm.Text,
                opt => opt.MapFrom(review => review.Text))
                .ForMember(reviewVm => reviewVm.CreatedAt,
                opt => opt.MapFrom(review => review.CreatedAt))
                .ForMember(reviewVm => reviewVm.User,
                opt => opt.MapFrom(review => review.User))
                .ForMember(reviewVm => reviewVm.Course,
                opt => opt.MapFrom(review => review.Course));
        }
    }
}
