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
    }
}
