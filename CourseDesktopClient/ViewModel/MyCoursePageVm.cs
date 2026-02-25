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
using System.Drawing.Printing;
using System.Reflection.Metadata;
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


        private int _complitedCourses;
        public int ComplitedCourses { get { return _complitedCourses; } set { _complitedCourses = value; OnPropertyChanged();} }
        private int _courseInPassage;
        public int CourseInPassage { get { return _courseInPassage; } set { _courseInPassage = value; OnPropertyChanged(); } }

        public ICommand ContinueCousre { get; set; }
        public ICommand ViewDetailsCommand { get; set; }
        public ICommand DeleteProgressCourseCommand { get; set; }
        public ICommand PagerCommand { get; set; }

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

        private readonly ICourseApiClient courseApiClient;
        private readonly IPagerService pagerService;
        private readonly IAuthService authService;

        public MyCoursePageVm(INavigationService navigationService , ICourseApiClient courseApiClient, IPagerService pagerService, IAuthService authService) : base(navigationService)
        {
            this.courseApiClient = courseApiClient;
            this.pagerService = pagerService;
            this.authService = authService;

            DeleteProgressCourseCommand = new RelayCommand(async button =>
            {
            if (CustomMessageBox.ShowYesNo($"Вы действительно хотите удавлить прогресс \nОн будет утерян навсегда") == DialogResult.Yes)
                {
                    var idProgerssUser = (button as MyCoursePanelElementVm).Id;

                    await courseApiClient.DeleteProgressUserAsync(idProgerssUser);

                    await Update();
                }
            });

            ContinueCousre = new RelayCommand( async sender =>
            {
                await navigationService.NavigateToProgressCourse((sender as MyCoursePanelElementVm).Id);
            });

            ViewDetailsCommand = new RelayCommand(async sender =>
            {
                var idCourse = (sender as MyCoursePanelElementVm).CourseId;
                await navigationService.NavigateToInformationCourse(idCourse);
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

        }

        public async Task LoadProgressUserForCoursesData(string searchText, int pageNumber)
        {
            var (progeresCourses, progressinfo, pager) = await courseApiClient.GetProgressUsersAsync(authService.CurrentUser.Id, pageNumber, searchText: searchText);

            var courseProgressViewModel = progeresCourses.ProgressUsers.Select(x => new MyCoursePanelElementVm(x)).ToList();

            ComplitedCourses = progressinfo.CompletedCourse;
            CourseInPassage = progressinfo.CourseInPassage;

            GetProgressUsers = courseProgressViewModel;
            GenerateButtonPanel(pager);

            VisibleButtonPanel = pager.TotalItems <= pager.PageSize
                ? Visibility.Collapsed
                : Visibility.Visible;

            VisibleEmptyPage = pager.TotalItems <= 0
                ? Visibility.Visible
                : Visibility.Collapsed;
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
