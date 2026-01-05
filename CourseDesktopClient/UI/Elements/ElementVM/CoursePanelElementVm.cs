using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.UI.Elements.ElementVM
{
    public class CoursePanelElementVm : ViewModelBase
    {
        private readonly ICourseApiClient courseApiClient;

        public string Title  { get; set; }
        public string? Description { get; set; }
        public Guid Id { get; set; }
        public UserDto User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public decimal? Rait { get; set; }

        private int _reviewsCount;
        public int ReviewsCount
        {
            get => _reviewsCount;
            set => SetProperty(ref _reviewsCount, value);
        }


        public CoursePanelElementVm(CourseDto courseDto, ICourseApiClient courseApiClient)
        {
            Title = courseDto.Title;
            Description = courseDto.Description;
            Id = courseDto.Id;
            User = courseDto.User;
            CreatedAt = courseDto.CreatedAt;
            UpdateAt = courseDto.UpdateAt;
            Rait = courseDto.Rait;

            Task.Run(async () =>
            {
                var (_, pager) = await courseApiClient.GetReviewsAsync(courseDto.Id);
                ReviewsCount = pager.TotalItems;
            });
            
            }

    }
}
