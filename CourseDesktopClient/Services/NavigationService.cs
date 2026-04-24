using CourseDesktopClient.Interfaces;
using CourseDesktopClient.View;
using CourseDesktopClient.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

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

        public void NavigateToRecoveryPassword()
        {
            var passwordRecoveryPage = serviceProvider.GetRequiredService<PasswordRecoveryPage>();
            NavigateTo(passwordRecoveryPage);
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



        public async Task NavigateToCreateUser()
        {
            var createUserPage = serviceProvider.GetRequiredService<CreateUserPage>();

            if (createUserPage.DataContext is CreateUserPageVm vm)
            {
                await vm.LoadCreateUserPage();
            }
            NavigateTo(createUserPage);
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

        public async Task NavigateToCreateCourse(Guid Id = default)
        {
            var createCoursePage = serviceProvider.GetRequiredService<CreateCoursePage>();

            if (createCoursePage.DataContext is CreateCoursePageVm vm)
            {
                await vm.LoadCourse(Id);
            }

            NavigateTo(createCoursePage);
        }

        public async Task NavigateToCreateModule(Guid courseId,Guid idModule = default)
        {
            var createModulePage = serviceProvider.GetRequiredService<CreateModulePage>();

            if (createModulePage.DataContext is CreateModulePageVm vm)
            {
                await vm.LoadModule(courseId, idModule);
            }

            NavigateTo(createModulePage);
        }
        public async Task NavigateToCreateMaterial(Guid moduleId, Guid idMaterial = default)
        {
            var createMaterialPage = serviceProvider.GetRequiredService<CreateMaterialPage>();

            if (createMaterialPage.DataContext is CreateMaterialPageVm vm)
            {
                await vm.LoadMaterial(idMaterial, moduleId);
            }

            NavigateTo(createMaterialPage);
        }

        public async Task NavigateToCreateTest(Guid idMaterial, Guid idTest = default)
        {
            var createTestPage = serviceProvider.GetRequiredService<CreateTestPage>();

            if (createTestPage.DataContext is CreateTestPageVm vm)
            {
                await vm.LoadTest(idTest, idMaterial);
            }

            NavigateTo(createTestPage);
        }

        public async Task NavigateToCreateQuestion(Guid idTest, Guid idQuestion = default)
        {
            var createQuestionPage = serviceProvider.GetRequiredService<CreateQuestionPage>();

            if (createQuestionPage.DataContext is CreateQuestionPageVm vm)
            {
                await vm.LoadQuestion(idQuestion, idTest);
            }

            NavigateTo(createQuestionPage);
        }

        public async Task NavigateToProgressCourse(Guid Id = default)
        {
            var compliteCoursePage = serviceProvider.GetRequiredService<CompletingCoursePage>();

            if (compliteCoursePage.DataContext is CompletingCoursePageVm vm)
            {
                await vm.LoadProgressCourse(Id);
            }

            NavigateTo(compliteCoursePage);
        }

        public async Task NavigateToResultsTest(Guid testId)
        {
            var testResultPage = serviceProvider.GetRequiredService<TestResultPage>();

            if (testResultPage.DataContext is TestResultPageVm vm)
            {
                await vm.LoadTestHistory(testId);
            }

            NavigateTo(testResultPage);
        }

        public async Task NavigateToProgressModule(Guid progressCourseId, Guid progressModuleId)
        {
            var completingModulePage = serviceProvider.GetRequiredService<CompletingModulePage>();

            if (completingModulePage.DataContext is CompletingModulePageVm vm)
            {
                await vm.LoadProgressModule(progressCourseId, progressModuleId);
            }

            NavigateTo(completingModulePage);
        }

        public async Task NavigateToProgressMaterial(Guid progressModuleId, Guid progressMaterialId)
        {
            var completingModulePage = serviceProvider.GetRequiredService<CompletingMaterialPage>();

            if (completingModulePage.DataContext is CompletingMaterialPageVm vm)
            {
                await vm.LoadProgressMaterial(progressModuleId, progressMaterialId);
            }

            NavigateTo(completingModulePage);
        }

        public async Task NavigateToTestResult(Guid progressMaterialId, Guid testResultId)
        {
            var completingTestResultPage = serviceProvider.GetRequiredService<CompletingTestResultPage>();

            if (completingTestResultPage.DataContext is CompletingTestResultPageVm vm)
            {
                await vm.LoadTestResult(progressMaterialId, testResultId);
            }

            NavigateTo(completingTestResultPage);
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