using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.Interfaces
{
    public interface INavigationService
    {
        void NavigateToLogin();
        void NavigateToRegister();
        Task NavigateToWorkshop();
        Task NavigateToCourses();
        Task NavigateToCreateCourse(Guid Id = default);
        Task NavigateToCreateModule(Guid courseId, Guid idModule = default);
        Task NavigateToCreateMaterial(Guid moduleId, Guid idMaterial = default);
        Task NavigateToCreateTest(Guid idMaterial, Guid idTest = default);
        Task NavigateToCreateQuestion(Guid idTest, Guid idQuestion = default);
        Task NavigateToProgressCourse(Guid Id = default);
        Task NavigateToProgressModule(Guid progressCourseId, Guid progressModuleId);
        Task NavigateToProgressMaterial(Guid progressModuleId, Guid progressMaterialId);
        Task NavigateToTestResult(Guid progressMaterialId, Guid testResultId);



        Task NavigateToInformationCourse(Guid Id);
        Task NavigateToProfile(Guid idUser);
        Task NavigateToMyCourses();
        Task NavigateToUsers();
        Task NavigateToCreateUser();
        void NavigateToUpdateUserPassword();
        void NavigateMistakePage(Exception exception);



        void GoBack();
        bool CanGoBack { get; }
        void ClearHistory();
    }
}
