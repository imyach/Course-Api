using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    public class NavigationVm(INavigationService navigationService) : ViewModelBase
    {

        public ICommand RegisterCommand { get; set; } = new RelayCommand(_ => navigationService.NavigateToRegister());
        public ICommand LoginCommand { get; set; } = new RelayCommand(_ => navigationService.NavigateToLogin());
        public ICommand AllCourseCommand { get; set; } = new RelayCommand(_ => navigationService.NavigateToCourses());
        public ICommand ProfileCommand { get; set; } = new RelayCommand(_ => navigationService.NavigateToProfile());
        public ICommand MyCourseCommand { get; set; } = new RelayCommand(_ => navigationService.NavigateMyCourseCommand());
    }
}
