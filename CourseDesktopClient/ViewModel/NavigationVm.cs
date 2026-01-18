using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Utilities;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    public class NavigationVm(INavigationService navigationService) : ViewModelBase
    {
        public ICommand GoBackCommand { get; set; } = new RelayCommand(execute: _ => navigationService.GoBack(), canExecute: _ => navigationService.CanGoBack);
        public ICommand RegisterCommand { get; set; } = new RelayCommand(_ => navigationService.NavigateToRegister());
        public ICommand LoginCommand { get; set; } = new RelayCommand(_ => navigationService.NavigateToLogin());
        public ICommand AllCourseCommand { get; set; } = new RelayCommand(_ => navigationService.NavigateToCourses());
        public ICommand MyCourseCommand { get; set; } = new RelayCommand(_ => navigationService.NavigateToMyCourses());
        public ICommand UsersPageCommand { get; set; } = new RelayCommand(_ => navigationService.NavigateToUsers());
        public ICommand UpdateUserPassword { get; set; } = new RelayCommand(_ => navigationService.NavigateToUpdateUserPassword());
    }
}
