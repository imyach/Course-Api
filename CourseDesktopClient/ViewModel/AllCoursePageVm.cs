using CourseDesktopClient.ApiConnection;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using CourseDesktopClient.Utilities;
using CredentialManagement;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    public class AllCoursePageVm : NavigationVm
    {
        private IList<CourseDto>? _getCourses;
        public IList<CourseDto>? GetCourses { get => _getCourses; set { _getCourses = value; OnPropertyChanged(); } }

        public ICommand LoadedCommand  => new RelayCommand(async _ => 
        {

            HttpResponseMessage response = await ClientConfig.Client.GetAsync(ApiPaths.API_GET_ALL_COURSE);

            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<CoursesDto>(jsondata);
                GetCourses = data?.Courses ?? throw new Exception("data is null");
            }
        });

    }
}
