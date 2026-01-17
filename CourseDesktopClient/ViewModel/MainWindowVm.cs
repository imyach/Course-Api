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
        public MainWindowVm(INavigationService navigationService, IAuthService authService ) : base(navigationService)
        {

            this.authService = authService;
            LogOutCommand = new RelayCommand(async _ =>
            {
                await authService.LogoutAsync();
            });
        }

        private string _search = string.Empty;
        public string Search
        {
            get => _search;
            set
            {
                if (SetProperty(ref _search, value))
                {
                    if (CurrentView is AllCoursePage coursePage &&
                        coursePage.DataContext is AllCoursePageVm courseVm )
                    {
                        courseVm.SearchCourse = value;
                    }
                    else if (CurrentView is MyCoursePage myCoursesPage &&
                       myCoursesPage.DataContext is MyCoursePageVm myCoursesVm)
                    {
                        myCoursesVm.SearchProgressCourse = value;
                    }
                    else if (CurrentView is AllUsersPage userPage &&
                       userPage.DataContext is AllUsersPageVm userVm)
                    {
                        userVm.SearchUsers = value;
                    }

                }
            }
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
        private string _usersText = string.Empty;
        public string UsersText
        {
            get { return _usersText; }
            set
            {
                _usersText = value;
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

        private Visibility _visibilityInAuthorizedProfile;
        public Visibility VisibilityInAuthorizedProfile
        {
            get { return _visibilityInAuthorizedProfile; }
            set
            {
                _visibilityInAuthorizedProfile = value;
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
        private Visibility _visibleControlPanel;
        public Visibility VisibleControlPanel
        {
            get { return _visibleControlPanel; }
            set
            {
                _visibleControlPanel = value;
                OnPropertyChanged();
            }
        }


        public ICommand LogOutCommand {  get; set; }

        private void UpddateVisible()
        {
            if (Application.Current.MainWindow.DataContext is MainWindowVm mainWindowVm)
            {
                if (authService.IsAuthenticated)
                {
                    UsersText = authService.CurrentUser.Role.Name is "Admin"
                        ? "Управление пользователями"
                        : "Пользователи";
                }

                VisibilitySearch = mainWindowVm.CurrentView is AllCoursePage or MyCoursePage or AllUsersPage
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                VisibilitySignInButton = !authService.IsAuthenticated && mainWindowVm.CurrentView is not LoginPage and not RegisterPage
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                VisibilityInAuthorizedProfile = authService.IsAuthenticated && mainWindowVm.CurrentView is not LoginPage and not RegisterPage
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                UserName = authService.IsAuthenticated
                   ? authService.CurrentUser.NameUser
                   : string.Empty;
            }
        }
    }
}
