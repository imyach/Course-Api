using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Services;
using CourseDesktopClient.UI.Elements.ElementVM;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    public class WorkshopPageVm : NavigationVm
    {
        private IList<CreateCoursePanelElementVm>? _getCourses;
        public IList<CreateCoursePanelElementVm>? GetCourses { get => _getCourses; set { _getCourses = value; OnPropertyChanged(nameof(GetCourses)); } }

        private ObservableCollection<ButtonItem>? _buttonPanel = [];
        public ObservableCollection<ButtonItem>? ButtonPanel { get => _buttonPanel; set { _buttonPanel = value; OnPropertyChanged(nameof(ButtonPanel)); } }

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

        public ICommand PagerCommand { get; set; }

        private readonly ICourseApiClient courseApiClient;
        private readonly IPagerService pagerService;
        private readonly IAuthService authService;

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

        public WorkshopPageVm(ICourseApiClient courseApiClient, INavigationService navigationService, IPagerService pagerService, IAuthService authService) : base(navigationService)
        {
            this.courseApiClient = courseApiClient;
            this.pagerService = pagerService;
            this.authService = authService;

            PagerCommand = new RelayCommand(async pageNumberStr =>
            {
                if (int.TryParse((pageNumberStr as ButtonItem).Text, out int pageNumber))
                {
                    await LoadingWorkshopPage(pageNumber);
                }
            });



        }

        public async Task LoadingWorkshopPage(int pageNumber = 1, int pageSize = 9)
        {
            var (courses, pager) = await courseApiClient.GetCreatedCoursesAsync(pageNumber, pageSize);

            var courseViewModel = courses.Courses.Select(x => new CreateCoursePanelElementVm(x)).ToList();
            GetCourses = courseViewModel;


            VisibleEmptyPage = pager.TotalItems <= 0
                ? Visibility.Visible
                : Visibility.Collapsed;

            VisibleButtonPanel = pager.TotalItems <= pager.PageSize
                ? Visibility.Collapsed
                : Visibility.Visible;

            GenerateButtonPanel(pager);
        }

        private void GenerateButtonPanel(PagerInfoDto pager)
        {
            var newButtonPanel = pagerService.GeneratePagerPanel(pager, PagerCommand);
            ButtonPanel = new ObservableCollection<ButtonItem>(newButtonPanel);
        }
    }
}
