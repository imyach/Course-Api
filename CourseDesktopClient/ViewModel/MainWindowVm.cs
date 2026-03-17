using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Services;
using CourseDesktopClient.UI.Elements.ElementVM;
using CourseDesktopClient.Utilities;
using CourseDesktopClient.View;
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
            DragWindowCommand = new RelayCommand(_ =>
            {
                if (Application.Current.MainWindow != null)
                {
                    Application.Current.MainWindow.DragMove();
                }
            });
            LogOutCommand = new RelayCommand(async _ =>
            {
                if(CustomMessageBox.ShowYesNo("Вы действительно хотите выйти?") == DialogResult.Yes) 
                    await authService.LogoutAsync();
            });
            ProfileCommand = new RelayCommand(async _ => 
            {
               await navigationService.NavigateToProfile(authService.CurrentUser.Id);
            });
            MinimizeWindowCommand = new RelayCommand(_ =>
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            });

            MaximizeWindowCommand = new RelayCommand(_ =>
            {
                Application.Current.MainWindow.WindowState =
                    Application.Current.MainWindow.WindowState == WindowState.Maximized
                    ? WindowState.Normal
                    : WindowState.Maximized;
            });

            CloseWindowCommand = new RelayCommand(_ =>
            {
                Application.Current.MainWindow.Close();
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

        private Visibility _couchAdminVisible;
        public Visibility CouchAdminVisible
        {
            get { return _couchAdminVisible; }
            set
            {
                _couchAdminVisible = value;
                OnPropertyChanged();
            }
        }

        private Visibility _misstakePageVisible;
        public Visibility MisstakePageVisible
        {
            get { return _misstakePageVisible; }
            set
            {
                _misstakePageVisible = value;
                OnPropertyChanged();
            }
        }

        public ICommand LogOutCommand {  get; set; }
        public ICommand ProfileCommand {  get; set; }
        public ICommand MinimizeWindowCommand { get; set; }
        public ICommand MaximizeWindowCommand { get; set; }
        public ICommand CloseWindowCommand { get; set; }
        public ICommand DragWindowCommand { get; set; }

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


                VisibleControlPanel = mainWindowVm.CurrentView is not MistakePage ? Visibility.Visible : Visibility.Collapsed;

                CouchAdminVisible = authService.IsAuthenticated && authService.CurrentUser.Role.Name is "Admin" or "Couch"
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                VisibilitySearch = mainWindowVm.CurrentView is AllCoursePage or MyCoursePage or AllUsersPage
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                VisibilitySignInButton = !authService.IsAuthenticated && mainWindowVm.CurrentView is not LoginPage and not RegisterPage and not MistakePage
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                VisibilityInAuthorizedProfile = authService.IsAuthenticated && mainWindowVm.CurrentView is not LoginPage and not RegisterPage
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                MisstakePageVisible = mainWindowVm.CurrentView is not MistakePage
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                UserName = authService.IsAuthenticated
                   ? authService.CurrentUser.NameUser
                   : string.Empty;
            }
        }
    }
}
