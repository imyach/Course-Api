using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.Interfaces
{
    public interface INavigationService
    {
        void NavigateToLogin();
        void NavigateToRegister();
        Task NavigateToCourses();
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
