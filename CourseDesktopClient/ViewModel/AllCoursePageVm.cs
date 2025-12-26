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

namespace CourseDesktopClient.ViewModel
{
    public class AllCoursePageVm : NavigationVm
    {
        private IList<CourseDto>? _getCourses;
        public IList<CourseDto>? GetCourses { get => _getCourses; set { _getCourses = value; OnPropertyChanged(nameof(GetCourses)); } }

        private ObservableCollection<ButtonItem>? _buttonPanel = [];
        public ObservableCollection<ButtonItem>? ButtonPanel{ get => _buttonPanel; set { _buttonPanel = value; OnPropertyChanged(nameof(ButtonPanel)); }}
        public ICommand PaginationCommand { get; set; }
        public ICommand LoadedCommand {  get; set; }

        private readonly ICourseApiClient courseApiClient;
        private readonly IPaginationService paginationService;

        public async Task LoadCoursesData(int pageNumber = 1)
        {
            var (courses, pager) = await courseApiClient.GetCoursesAsync(pageNumber);
            GetCourses = courses.Courses;
            GeneratePaginationPanel(pager); 
        }

        public void GeneratePaginationPanel(PagerInfoDto pager)
        {
            var newButtonPanel = paginationService.GeneratePaginationPanel(pager, PaginationCommand);
            ButtonPanel = new ObservableCollection<ButtonItem>(newButtonPanel);
        }


        public AllCoursePageVm(ICourseApiClient courseApiClient, INavigationService navigationService, IPaginationService paginationService) : base(navigationService)
        {
            this.courseApiClient = courseApiClient;
            this.paginationService = paginationService;

            PaginationCommand = new RelayCommand(pageNumberStr =>
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
        }
    }
}
