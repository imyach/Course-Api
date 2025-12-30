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
            var coursesPage = serviceProvider.GetRequiredService<AllCoursePage>();
            SetMainWindowContent(coursesPage);
        }

        public async void NavigateToInformationCourse(Guid Id)
        {
            var coursePage = serviceProvider.GetRequiredService<CourseInformationPage>();

            if(coursePage.DataContext is CourseInformationPageVm vm)
            {
                await vm.LoadCourse(Id);
            }
            SetMainWindowContent(coursePage);
        }

        public void NavigateToLogin()
        {
            var loginPage = serviceProvider.GetRequiredService<LoginPage>();
            SetMainWindowContent(loginPage);
        }

        public void NavigateToProfile()
        {
           var profilePage = serviceProvider.GetRequiredService<ProfilePage>();
           SetMainWindowContent(profilePage);
        }

        public void NavigateToRegister()
        {
            var registerPage = serviceProvider.GetRequiredService<RegisterPage>();
            SetMainWindowContent(registerPage);
        }

        private void SetMainWindowContent(object content)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow?.DataContext is MainWindowVm mainWindowVm)
            {
                mainWindowVm.CurrentView = content;
            }
        }
    }
}
