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
using System.Windows.Controls;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    public class MyCoursePageVm : NavigationVm
    {
        private IList<MyCoursePanelElementVm>? _getProgressUsers;
        public IList<MyCoursePanelElementVm>? GetProgressUsers { get => _getProgressUsers; set { _getProgressUsers = value; OnPropertyChanged(nameof(GetProgressUsers)); } }

        private ObservableCollection<ButtonItem>? _buttonPanel = [];
        public ObservableCollection<ButtonItem>? ButtonPanel { get => _buttonPanel; set { _buttonPanel = value; OnPropertyChanged(nameof(ButtonPanel)); } }
        private string _searchProgressCourse = string.Empty;
        public string SearchProgressCourse { get { return _searchProgressCourse; } set { _searchProgressCourse = value; SetProperty(ref _searchProgressCourse, value); Update(); } }

        public ICommand ContinueCousre { get; set; }
        public ICommand ViewDetailsCommand { get; set; }
        public ICommand DeleteProgressCourseCommand { get; set; }
        public ICommand PagerCommand { get; set; }

        private readonly ICourseApiClient courseApiClient;
        private readonly IPagerService pagerService;

        public MyCoursePageVm(INavigationService navigationService , ICourseApiClient courseApiClient, IPagerService pagerService) : base(navigationService)
        {
            this.courseApiClient = courseApiClient;
            this.pagerService = pagerService;

            DeleteProgressCourseCommand = new RelayCommand(async button =>
            {
                if (MessageBox.Show("Вы действительно хотите удавлить прогресс?", "Предупреждение!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    var idProgerssUser = (button as MyCoursePanelElementVm).Id;

                    await courseApiClient.DeleteProgressUserAsync(idProgerssUser);

                    await Update();
                }
            });

            ContinueCousre = new RelayCommand(_ =>
            {
                ///
            });

            ViewDetailsCommand = new RelayCommand(async sender =>
            {
                var idCourse = (sender as MyCoursePanelElementVm).CourseId;
                navigationService.NavigateToInformationCourse(idCourse);
            });
            PagerCommand = new RelayCommand(async pageNumberStr =>
            {
                if (int.TryParse((pageNumberStr as ButtonItem).Text, out int pageNumber))
                {
                    await Update(pageNumber);

                }
            });

        }

        public async Task LoadProgressUserForCoursesData(string searchText, int pageNumber)
        {
            var (progeresCourses, pager) = await courseApiClient.GetProgressUsersAsync(pageNumber, searchText: searchText);

            var courseProgressViewModel = progeresCourses.ProgressUsers.Select(x => new MyCoursePanelElementVm(x)).ToList();

            GetProgressUsers = courseProgressViewModel;
            GenerateButtonPanel(pager);
        }

        private void GenerateButtonPanel(PagerInfoDto pager)
        {
            var newButtonPanel = pagerService.GeneratePagerPanel(pager, PagerCommand);
            ButtonPanel = new ObservableCollection<ButtonItem>(newButtonPanel);
        }

        public async Task Update(int pageNumber = 1)
        {
            await LoadProgressUserForCoursesData(SearchProgressCourse, pageNumber);
        }


    }
}
