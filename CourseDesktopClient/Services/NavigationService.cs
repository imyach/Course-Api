using CourseDesktopClient.Interfaces;
using CourseDesktopClient.View;
using CourseDesktopClient.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CourseDesktopClient.Services
{
    public class NavigationService(IServiceProvider serviceProvider) : INavigationService
    {
        public void NavigateToCourses()
        {
            var coursePage = serviceProvider.GetRequiredService<AllCoursePage>();
            SetMainWindowContent(coursePage);
        }

        public void NavigateToLogin()
        {
            var loginPage = serviceProvider.GetRequiredService<LoginPage>();
            SetMainWindowContent(loginPage);
        }

        public void NavigateToRegister()
        {
            var registerPage = serviceProvider.GetRequiredService<RegisterPage>();
            SetMainWindowContent(registerPage);
        }

        private static void SetMainWindowContent(object content)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow?.DataContext is MainWindowVm mainWindowVm)
            {
                mainWindowVm.CurrentView = content;
            }
        }
    }
}
