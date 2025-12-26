using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    public class NavigationVm(INavigationService navigationService) : ViewModelBase
    {
        public ICommand RegisterCommand { get; set; } = new RelayCommand(_ => navigationService.NavigateToRegister());
        public ICommand LoginCommand { get; set; } = new RelayCommand(_ => navigationService.NavigateToLogin());
        public ICommand AllCourseCommand { get; set; } = new RelayCommand(_ => navigationService.NavigateToCourses());
    }
}
