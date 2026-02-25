using CourseDesktopClient.Api;
using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.Entities.RequestDto;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using CourseDesktopClient.Services;
using CourseDesktopClient.UI.Elements.ElementVM;
using CourseDesktopClient.Utilities;
using CredentialManagement;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
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
        public ICommand UpdateCourseCommand {  get; set; }

        private readonly ICourseApiClient courseApiClient;
        private readonly IPagerService pagerService;
        private readonly IAuthService authService;

        private Visibility _visibleButtonPanel;
        public Visibility VisibleButtonPanel
        {
            get { return _visibleButtonPanel; }
            set
            {
                _visibleButtonPanel = value;
                OnPropertyChanged();
            }
        }

        private Visibility _visibleEmptyPage;
        public Visibility VisibleEmptyPage
        {
            get { return _visibleEmptyPage; }
            set
            {
                _visibleEmptyPage = value;
                OnPropertyChanged();
            }
        }

        private void GenerateButtonPanel(PagerInfoDto pager)
        {
            var newButtonPanel = pagerService.GeneratePagerPanel(pager, PagerCommand);
            ButtonPanel = new ObservableCollection<ButtonItem>(newButtonPanel);
        }


        public AllCoursePageVm(ICourseApiClient courseApiClient, INavigationService navigationService, IPagerService pagerService, IAuthService authService) : base(navigationService)
        {
            this.courseApiClient = courseApiClient;
            this.pagerService = pagerService;
            this.authService = authService;

            UpdateCourseCommand = new RelayCommand(async sender =>
            {
                await navigationService.NavigateToCreateCourse((sender as CoursePanelElementVm).Id);
            });

            PagerCommand = new RelayCommand(async parameter =>
            {
                if (parameter is ButtonItem buttonItem && int.TryParse(buttonItem.Text, out int pageNumber))
                {
                    // Обновляем IsSelected для всех кнопок
                    if (ButtonPanel != null)
                    {
                        foreach (var btn in ButtonPanel)
                        {
                            btn.IsSelected = btn.Text == pageNumber.ToString();
                        }
                    }

                    await Update(pageNumber);
                }
            });

            ViewDetailsCommand = new RelayCommand(async sender =>
            {
                if (!authService.IsAuthenticated)
                {
                    CustomMessageBox.ShowError("Необходимо авторизироваться");
                }
                else
                {
                    var idCourse = (sender as CoursePanelElementVm).Id;
                    await navigationService.NavigateToInformationCourse(idCourse);
                }
            });

            EnrollCommand = new RelayCommand(async sender => 
            {
                if (!authService.IsAuthenticated)
                {
                    CustomMessageBox.ShowError("Необходимо аторизироваться");
                }
                else
                {
                    var idCourse = (sender as CoursePanelElementVm).Id;
                    var titleCourse = (sender as CoursePanelElementVm).Title;
                    var request = new ProgressUserRequestDto
                    {
                        CourseId = idCourse,
                    };
                    var id = await courseApiClient.CreateProgressUserAsync(request);

                    CustomMessageBox.ShowInfo($"Вы записаны на курс {titleCourse}");
                    await Update();
                }
            });

        }
        public async Task Update(int pageNumber = 1)
        {
            var (courses, pager) = await courseApiClient.GetCoursesAsync(pageNumber, searchText: SearchCourse);

            if (authService.IsAuthenticated)
            {
                var (progress, _, _) = await courseApiClient.GetProgressUsersAsync(authService.CurrentUser.Id, pageSize: int.MaxValue);
                var courseViewModel = courses.Courses.Select(x => new CoursePanelElementVm(x, courseApiClient, progress, authService)).ToList();
                GetCourses = courseViewModel;
            }
            else
            {
                var courseViewModel = courses.Courses.Select(x => new CoursePanelElementVm(x, courseApiClient, null, null)).ToList();
                GetCourses = courseViewModel;
            }
            GenerateButtonPanel(pager);

            VisibleButtonPanel = pager.TotalItems <= pager.PageSize
                ? Visibility.Collapsed
                : Visibility.Visible;

            VisibleEmptyPage = pager.TotalItems <= 0
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }
}
