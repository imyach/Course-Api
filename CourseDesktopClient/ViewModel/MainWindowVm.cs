using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Utilities;
using CourseDesktopClient.View;
using CredentialManagement;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    public class MainWindowVm :NavigationVm
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value;
                OnPropertyChanged(); }
        }

        public ICommand LogOutCommand {  get; set; }

        public MainWindowVm(INavigationService navigationService, IAuthService authService) : base(navigationService) 
        {
            LogOutCommand = new RelayCommand(async _ =>
            {
                await authService.LogoutAsync();
            });
        }
    }
}
