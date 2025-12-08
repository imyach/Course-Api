using CourseDesktopClient.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CourseDesktopClient.Utilities
{
    public class NavigationVm : ViewModelBase
    {
        private  object? _currentView;
        public  object? CurrentView { get => _currentView;  set { _currentView = value;OnPropertyChanged(); } }
        private Visibility _visibilitySearch;
        public Visibility VisibilitySearch { get => _visibilitySearch; set { _visibilitySearch = value; OnPropertyChanged(); } }

        public ICommand? RegisterCommand { get; set; }
        public ICommand? LoginCommand { get; set; }
        public ICommand? AllCourseCommand { get; set; }

        public static void RegisterCommandFunc() { Nav.Current?.CurrentView = new RegisterPage(); Nav.Current?.VisibilitySearch = Visibility.Collapsed; }
        public static void LoginCommandFunc() { Nav.Current?.CurrentView = new LoginPage(); Nav.Current?.VisibilitySearch = Visibility.Collapsed; }
        public static void AllCourseCommandFunc() { Nav.Current?.CurrentView = new AllCoursePage(); Nav.Current?.VisibilitySearch = Visibility.Visible; }


        public NavigationVm()
        {
            RegisterCommand = new RelayCommand(x => RegisterCommandFunc());
            LoginCommand = new RelayCommand(x => LoginCommandFunc());
            AllCourseCommand = new RelayCommand(x => AllCourseCommandFunc());
        }
    }
}
