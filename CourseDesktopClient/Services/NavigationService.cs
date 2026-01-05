using CourseDesktopClient.Interfaces;
using CourseDesktopClient.View;
using CourseDesktopClient.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CourseDesktopClient.Services
{
    public class NavigationService(IServiceProvider serviceProvider) : INavigationService
    {
        private readonly Stack<object> _navigationStack = new Stack<object>();

        public async void NavigateToCourses()
        {
            var coursesPage = serviceProvider.GetRequiredService<AllCoursePage>();

            if (coursesPage.DataContext is AllCoursePageVm vm)
            {
                if (Application.Current.MainWindow?.DataContext is MainWindowVm mainVm)
                {
                    vm.SearchCourse = mainVm.SearchCourse;
                }
                await vm.Update();
            }
            NavigateTo(coursesPage);
        }

        public async void NavigateToInformationCourse(Guid Id)
        {
            var coursePage = serviceProvider.GetRequiredService<CourseInformationPage>();

            if (coursePage.DataContext is CourseInformationPageVm vm)
            {
                await vm.LoadCourse(Id);
            }

            NavigateTo(coursePage);
        }

        public void NavigateToLogin()
        {
            var loginPage = serviceProvider.GetRequiredService<LoginPage>();
            NavigateTo(loginPage);
        }

        public void NavigateToProfile()
        {
            var profilePage = serviceProvider.GetRequiredService<ProfilePage>();

            if (profilePage.DataContext is ProfilePageVm vm)
            {
                vm.LoadingProfilePage();
            }
            NavigateTo(profilePage);
        }

        public void NavigateToRegister()
        {
            var registerPage = serviceProvider.GetRequiredService<RegisterPage>();
            NavigateTo(registerPage);
        }

        public bool CanGoBack => _navigationStack.Count > 1;

        public void GoBack()
        {
            if (!CanGoBack) return;

            // Удаляем текущую страницу
            _navigationStack.Pop();

            // Берем предыдущую страницу
            var previousPage = _navigationStack.Peek();

            SetMainWindowContent(previousPage);
        }

        private void NavigateTo(object content)
        {
            // Добавляем в стек (если это не та же самая страница)
            if (_navigationStack.Count == 0 || _navigationStack.Peek() != content)
            {
                _navigationStack.Push(content);
            }

            SetMainWindowContent(content);
        }

        private void SetMainWindowContent(object content)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow?.DataContext is MainWindowVm mainWindowVm)
            {
                mainWindowVm.CurrentView = content;
            }
        }

        // Метод для очистки стека (например, при выходе)
        public void ClearHistory()
        {
            _navigationStack.Clear();
        }

        public void NavigateMyCourseCommand()
        {
           //////
        }
    }
}