using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace CourseDesktopClient.UI.Elements.ElementVM
{
    class ReviewPanelElementVm(ReviewDto reviewDto, Guid currentUserId)  :ViewModelBase
    {
        public Guid Id => reviewDto.Id;
        public string Text => reviewDto.Text;
        public int Rait => reviewDto.Rait;
        public DateTime CreatedAt => reviewDto.CreatedAt;
        public UserDto User => reviewDto.User;

        public bool IsCurrentUserReview => reviewDto.User?.Id == currentUserId;

        public ReviewDto GetReviewDto() => reviewDto;
    }
}
