using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using CourseDesktopClient.Services;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CourseDesktopClient.UI.Elements.ElementVM
{
    public class CoursePanelElementVm : ViewModelBase
    {
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

        private bool _isEnrolled = false;
        public bool IsEnrolled
        {
            get => _isEnrolled;
            set => SetProperty(ref _isEnrolled, value);
        }

        private Visibility _updateCourseAdminVisible;
        public Visibility UpdateCourseAdminVisible
        {
            get { return _updateCourseAdminVisible; }
            set
            {
                _updateCourseAdminVisible = value;
                OnPropertyChanged();
            }
        }

        public CoursePanelElementVm(CourseDto courseDto, ICourseApiClient courseApiClient, ProgressUsersDto? progress, IAuthService? authService)
        {
            Title = courseDto.Title;
            Description = courseDto.Description;
            Id = courseDto.Id;
            User = courseDto.User;
            CreatedAt = courseDto.CreatedAt;
            UpdateAt = courseDto.UpdateAt;
            Rait = courseDto.Rait;
            UpdateCourseAdminVisible = authService != null && (authService.CurrentUser.Role.Name == "Admin" || authService.CurrentUser.Id == User.Id) ? Visibility.Visible : Visibility.Collapsed;

            Task.Run(async () =>
            {
                var (_, pager) = await courseApiClient.GetReviewsAsync(courseDto.Id);
                ReviewsCount = pager.TotalItems;
            });
            
            if(progress != null && authService != null)
                IsEnrolled = progress.ProgressUsers?.Any(pu=> pu.Course.Id == Id && pu.User.Id == authService.CurrentUser.Id) ?? false;
        }
    }
}
