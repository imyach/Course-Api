using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourseDesktopClient.UI.Elements.ElementVM
{
    class ReviewPanelElementVm(ReviewDto reviewDto, UserDto currentUser)  :ViewModelBase
    {
        public Guid Id => reviewDto.Id;
        public string Text => reviewDto.Text;
        public int Rait => reviewDto.Rait;
        public DateTime CreatedAt => reviewDto.CreatedAt;
        public UserDto User => reviewDto.User;

        public bool IsCurrentUserReview => reviewDto.User?.Id == currentUser.Id || currentUser.Role.Name == "Admin";

    }
}
