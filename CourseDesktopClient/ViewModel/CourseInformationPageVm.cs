using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.ViewModel
{
    class CourseInformationPageVm : NavigationVm
    {
        public CourseInformationPageVm(INavigationService navigationService, ICourseApiClient courseApiClient) :base(navigationService)
        {
            this.courseApiClient = courseApiClient;
        }

        private readonly ICourseApiClient courseApiClient;
        public async Task LoadCourse(Guid id)
        {
            var infoCourse = await courseApiClient.GetCourseByIdAsync(id);
        }
    }
}
