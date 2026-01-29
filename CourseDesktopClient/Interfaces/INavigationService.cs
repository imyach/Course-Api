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
        Task NavigateToInformationCourse(Guid Id);
        Task NavigateToMyCourses();
        Task NavigateToProfile(Guid idUser);
        Task NavigateToUsers();
        void NavigateToUpdateUserPassword();
        void NavigateMistakePage(Exception exception);



        void GoBack();
        bool CanGoBack { get; }
        void ClearHistory();
    }
}
