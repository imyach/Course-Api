using CourseDesktopClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.ViewModel
{
    public class AllUsersPageVm : NavigationVm
    {
        private string _searchProgressCourse = string.Empty;
        public string SearchUsers { get { return _searchProgressCourse; } set { _searchProgressCourse = value; SetProperty(ref _searchProgressCourse, value); Update(); } }

        public AllUsersPageVm(INavigationService navigationService):base(navigationService)
        {
            
        }

        public async Task Update()
        {

        }
    }
}
