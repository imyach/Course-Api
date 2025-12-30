using CourseDesktopClient.Api;
using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using CourseDesktopClient.Services;
using CourseDesktopClient.Utilities;
using CredentialManagement;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace CourseDesktopClient.ViewModel
{
    public class AllCoursePageVm : NavigationVm
    {
        private IList<CourseDto>? _getCourses;
        public IList<CourseDto>? GetCourses { get => _getCourses; set { _getCourses = value; OnPropertyChanged(nameof(GetCourses)); } }

        private ObservableCollection<ButtonItem>? _buttonPanel = [];
        public ObservableCollection<ButtonItem>? ButtonPanel{ get => _buttonPanel; set { _buttonPanel = value; OnPropertyChanged(nameof(ButtonPanel)); }}
        public ICommand PagerCommand { get; set; }
        public ICommand LoadedCommand {  get; set; }
        public ICommand ViewDetailsCommand {  get; set; }

        private readonly ICourseApiClient courseApiClient;
        private readonly IPagerService pagerService;

        public async Task LoadCoursesData(int pageNumber = 1)
        {
            var (courses, pager) = await courseApiClient.GetCoursesAsync(pageNumber);
            GetCourses = courses.Courses;
            GenerateButtonPanel(pager); 
        }

        public void GenerateButtonPanel(PagerInfoDto pager)
        {
            var newButtonPanel = pagerService.GeneratePagerPanel(pager, PagerCommand);
            ButtonPanel = new ObservableCollection<ButtonItem>(newButtonPanel);
        }


        public AllCoursePageVm(ICourseApiClient courseApiClient, INavigationService navigationService, IPagerService pagerService) : base(navigationService)
        {
            this.courseApiClient = courseApiClient;
            this.pagerService = pagerService;

            PagerCommand = new RelayCommand(pageNumberStr =>
            {
                if (int.TryParse((pageNumberStr as ButtonItem).Text, out int pageNumber))
                {
                    LoadCoursesData(pageNumber);

                }
            });

            LoadedCommand = new RelayCommand(async _ =>
            {
                await LoadCoursesData();
            });


            ViewDetailsCommand = new RelayCommand(async sender =>
            {
                var idCourse = (sender as CourseDto).Id;
                navigationService.NavigateToInformationCourse(idCourse);
            });

    }
    }
}
