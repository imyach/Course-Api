using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.Interfaces
{
    public interface INavigationService
    {
        void NavigateToLogin();
        void NavigateToRegister();
        void NavigateToCourses();
        void NavigateToMyCourses();
        void NavigateToProfile();
        void NavigateToUsers();
        void NavigateToInformationCourse(Guid Id);
        void NavigateToUpdateUserPassword();
        void NavigateMistakePage(Exception exception);



        void GoBack();
        bool CanGoBack { get; }
        void ClearHistory();
    }
}
