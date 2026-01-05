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
        void NavigateToProfile();
        void NavigateMyCourseCommand();
        void NavigateToInformationCourse(Guid Id);


        void GoBack();
        bool CanGoBack { get; }
        void ClearHistory();
    }
}
