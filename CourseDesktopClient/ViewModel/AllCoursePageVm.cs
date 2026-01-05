using CourseDesktopClient.Api;
using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using CourseDesktopClient.Services;
using CourseDesktopClient.UI.Elements.ElementVM;
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
        private IList<CoursePanelElementVm>? _getCourses;
        public IList<CoursePanelElementVm>? GetCourses { get => _getCourses; set { _getCourses = value; OnPropertyChanged(nameof(GetCourses)); } }

        private ObservableCollection<ButtonItem>? _buttonPanel = [];
        public ObservableCollection<ButtonItem>? ButtonPanel{ get => _buttonPanel; set { _buttonPanel = value; OnPropertyChanged(nameof(ButtonPanel)); }}
        private string _searchCourse = string.Empty;
        public string SearchCourse { get { return _searchCourse; } set { _searchCourse =  value; SetProperty(ref _searchCourse,  value);  Update(); } }
        public ICommand PagerCommand { get; set; }
        public ICommand EnrollCommand {  get; set; }
        public ICommand ViewDetailsCommand {  get; set; }

        private readonly ICourseApiClient courseApiClient;
        private readonly IPagerService pagerService;

        public async Task LoadCoursesData( string searchText, int pageNumber)
        {
            var (courses, pager) = await courseApiClient.GetCoursesAsync(pageNumber, searchText: searchText);

            var courseViewModel = courses.Courses.Select(x => new CoursePanelElementVm(x, courseApiClient)).ToList();

            GetCourses = courseViewModel;
            GenerateButtonPanel(pager); 
        }

        private void GenerateButtonPanel(PagerInfoDto pager)
        {
            var newButtonPanel = pagerService.GeneratePagerPanel(pager, PagerCommand);
            ButtonPanel = new ObservableCollection<ButtonItem>(newButtonPanel);
        }


        public AllCoursePageVm(ICourseApiClient courseApiClient, INavigationService navigationService, IPagerService pagerService) : base(navigationService)
        {
            this.courseApiClient = courseApiClient;
            this.pagerService = pagerService;

            PagerCommand = new RelayCommand(async pageNumberStr =>
            {
                if (int.TryParse((pageNumberStr as ButtonItem).Text, out int pageNumber))
                {
                    await Update(pageNumber);

                }
            });

            ViewDetailsCommand = new RelayCommand(async sender =>
            {
                var idCourse = (sender as CoursePanelElementVm).Id;
                navigationService.NavigateToInformationCourse(idCourse);
            });

            EnrollCommand = new RelayCommand(async sender => 
            {
                //////////////////////////////////////
            });

        }

        public async Task Update(int pageNumber = 1)
        {
            await LoadCoursesData(SearchCourse, pageNumber);
        }
    }
}
