using CourseDesktopClient.Interfaces;
using CourseDesktopClient.View;
using CourseDesktopClient.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CourseDesktopClient.Services
{
    public class NavigationService(IServiceProvider serviceProvider) : INavigationService
    {
        private readonly Stack<object> _navigationStack = new Stack<object>();

        public async Task NavigateToCourses()
        {
            var coursesPage = serviceProvider.GetRequiredService<AllCoursePage>();

            if (coursesPage.DataContext is AllCoursePageVm vm)
            {
                if (Application.Current.MainWindow?.DataContext is MainWindowVm mainVm)
                {
                    vm.SearchCourse = mainVm.Search;
                }
                await vm.Update();
            }
            NavigateTo(coursesPage);
        }

        public async Task NavigateToInformationCourse(Guid Id)
        {
            var coursePage = serviceProvider.GetRequiredService<CourseInformationPage>();

            if (coursePage.DataContext is CourseInformationPageVm vm)
            {
                await vm.LoadCourse(Id);
            }

            NavigateTo(coursePage);
        }
        public async Task NavigateToMyCourses()
        {
            var myCoursePage = serviceProvider.GetRequiredService<MyCoursePage>();

            if (myCoursePage.DataContext is MyCoursePageVm vm)
            {
                if (Application.Current.MainWindow?.DataContext is MainWindowVm mainVm)
                {
                    vm.SearchProgressCourse = mainVm.Search;
                }
                await vm.Update();
            }
            NavigateTo(myCoursePage);
        }
        public async Task NavigateToUsers()
        {
            var allUsersPage = serviceProvider.GetRequiredService<AllUsersPage>();

            if (allUsersPage.DataContext is AllUsersPageVm vm)
            {
                if (Application.Current.MainWindow?.DataContext is MainWindowVm mainVm)
                {
                    vm.SearchUsers = mainVm.Search;
                }
                await vm.Update();
            }
            NavigateTo(allUsersPage);
        }
        public void NavigateToUpdateUserPassword()
        {
            var updUserPage = serviceProvider.GetRequiredService<UpdateUserPasswordPage>();
            NavigateTo(updUserPage);
        }

        public void NavigateMistakePage(Exception exception)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var mistakePage = serviceProvider.GetRequiredService<MistakePage>();

                if (mistakePage.DataContext is MistakePageVm vm)
                {
                    vm.SetException(exception);
                }

                _navigationStack.Clear();

                SetMainWindowContent(mistakePage);
            });

        }

        public void NavigateToLogin()
        {
            var loginPage = serviceProvider.GetRequiredService<LoginPage>();
            NavigateTo(loginPage);
        }

        public async Task NavigateToProfile(Guid idUser)
        {
            var profilePage = serviceProvider.GetRequiredService<ProfilePage>();

            if (profilePage.DataContext is ProfilePageVm vm)
            {
                await vm.LoadingProfilePage(idUser);
            }
            NavigateTo(profilePage);
        }

        public void NavigateToRegister()
        {
            var registerPage = serviceProvider.GetRequiredService<RegisterPage>();
            NavigateTo(registerPage);
        }

        public async Task NavigateToWorkshop()
        {
            var workshopPage = serviceProvider.GetRequiredService<WorkshopPage>();

            if (workshopPage.DataContext is WorkshopPageVm vm)
            {
                await vm.LoadingWorkshopPage();
            }
            NavigateTo(workshopPage);
        }



        public bool CanGoBack => _navigationStack.Count > 1;

        public void GoBack()
        {
            if (!CanGoBack) return;

            _navigationStack.Pop();

            var previousPage = _navigationStack.Peek();

            SetMainWindowContent(previousPage);
        }

        private void NavigateTo(object content)
        {
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

        public void ClearHistory()
        {
            _navigationStack.Clear();
        }

        
    }
}