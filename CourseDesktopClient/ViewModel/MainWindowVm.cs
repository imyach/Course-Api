using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Services;
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
        public MainWindowVm(INavigationService navigationService, IAuthService authService) : base(navigationService)
        {
            this.authService = authService;
            LogOutCommand = new RelayCommand(async _ =>
            {
                await authService.LogoutAsync();
            });
        }
        private readonly IAuthService authService;

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value;
                OnPropertyChanged();
                UpddateVisible();
            }
        }

        private string _userName = string.Empty;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }
        private Visibility _visibilitySearch;
        public Visibility VisibilitySearch
        {
            get { return _visibilitySearch; }
            set
            {
                _visibilitySearch = value;
                OnPropertyChanged();
            }
        }

        private Visibility _visibilityProfileButton;
        public Visibility VisibilityProfileButton
        {
            get { return _visibilityProfileButton; }
            set
            {
                _visibilityProfileButton = value;
                OnPropertyChanged();
            }
        }

        private Visibility _visibilitySignInButton;
        public Visibility VisibilitySignInButton
        {
            get { return _visibilitySignInButton; }
            set
            {
                _visibilitySignInButton = value;
                OnPropertyChanged();
            }
        }


        public ICommand LogOutCommand {  get; set; }

        private void UpddateVisible()
        {
            if (Application.Current.MainWindow.DataContext is MainWindowVm mainWindowVm)
            {
                VisibilitySearch = mainWindowVm.CurrentView is AllCoursePage
                    ? Visibility.Visible 
                    : Visibility.Collapsed;
                
                VisibilitySignInButton = !authService.IsAuthenticated && mainWindowVm.CurrentView is not LoginPage and not RegisterPage
                    ? Visibility.Visible 
                    : Visibility.Collapsed;

                VisibilityProfileButton = authService.IsAuthenticated && mainWindowVm.CurrentView is not LoginPage and not RegisterPage
                    ? Visibility.Visible 
                    : Visibility.Collapsed;

                UserName = authService.IsAuthenticated 
                    ? authService.CurrentUser.NameUser
                    : string.Empty;
            }
        }
    }
}
