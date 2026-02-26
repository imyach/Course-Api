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
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    public class WorkshopPageVm : NavigationVm
    {
        private IList<CreateCoursePanelElementVm>? _getCourses;
        public IList<CreateCoursePanelElementVm>? GetCourses
        {
            get => _getCourses;
            set
            {
                _getCourses = value;
                OnPropertyChanged(nameof(GetCourses));
                UpdateStatistics();
            }
        }

        private ObservableCollection<ButtonItem>? _buttonPanel = [];
        public ObservableCollection<ButtonItem>? ButtonPanel
        {
            get => _buttonPanel;
            set
            {
                _buttonPanel = value;
                OnPropertyChanged(nameof(ButtonPanel));
            }
        }

        private Visibility _visibleButtonPanel;
        public Visibility VisibleButtonPanel
        {
            get => _visibleButtonPanel;
            set
            {
                _visibleButtonPanel = value;
                OnPropertyChanged();
            }
        }

        private Visibility _visibleEmptyPage;
        public Visibility VisibleEmptyPage
        {
            get => _visibleEmptyPage;
            set
            {
                _visibleEmptyPage = value;
                OnPropertyChanged();
            }
        }

        private Visibility _visibleAddButton;
        public Visibility VisibleAddButton
        {
            get => _visibleAddButton;
            set
            {
                _visibleAddButton = value;
                OnPropertyChanged();
            }
        }


        private int _totalCourses;
        public int TotalCourses
        {
            get => _totalCourses;
            set
            {
                _totalCourses = value;
                OnPropertyChanged();
            }
        }

        private int _draftCourses;
        public int DraftCourses
        {
            get => _draftCourses;
            set
            {
                _draftCourses = value;
                OnPropertyChanged();
            }
        }

        private int _publishedCourses;
        public int PublishedCourses
        {
            get => _publishedCourses;
            set
            {
                _publishedCourses = value;
                OnPropertyChanged();
            }
        }

        private bool _hasPreviousPage;
        public bool HasPreviousPage
        {
            get => _hasPreviousPage;
            set
            {
                _hasPreviousPage = value;
                OnPropertyChanged();
            }
        }

        private bool _hasNextPage;
        public bool HasNextPage
        {
            get => _hasNextPage;
            set
            {
                _hasNextPage = value;
                OnPropertyChanged();
            }
        }


        public ICommand PagerCommand { get; set; }
        public ICommand LocalCreateCouseCommand { get; set; }
        public ICommand DeleteCourseCommand { get; set; }

        private readonly ICourseApiClient courseApiClient;
        private readonly IPagerService pagerService;
        private readonly IAuthService authService;
        private readonly INavigationService navigationService;

        public WorkshopPageVm(
            ICourseApiClient courseApiClient,
            INavigationService navigationService,
            IPagerService pagerService,
            IAuthService authService) : base(navigationService)
        {
            this.courseApiClient = courseApiClient;
            this.navigationService = navigationService;
            this.pagerService = pagerService;
            this.authService = authService;

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            PagerCommand = new RelayCommand(async parameter =>
            {
                if (parameter is ButtonItem buttonItem && int.TryParse(buttonItem.Text, out int pageNumber))
                {
                    if (ButtonPanel != null)
                    {
                        foreach (var btn in ButtonPanel)
                        {
                            btn.IsSelected = btn.Text == pageNumber.ToString();
                        }
                    }
                    await LoadingWorkshopPage(pageNumber);
                }
            });

            LocalCreateCouseCommand = new RelayCommand(async sender =>
            {
                if (sender is CreateCoursePanelElementVm courseVm)
                {
                    await navigationService.NavigateToCreateCourse(courseVm.Id);
                }
            });

            DeleteCourseCommand = new RelayCommand(async sender =>
            {
                if (sender is CreateCoursePanelElementVm courseVm)
                {
                    var result = CustomMessageBox.ShowYesNo("Вы действительно хотите удалить этот курс?");
                    if (result == DialogResult.Yes)
                    {
                        await courseApiClient.DeleteCourseAsync(courseVm.Id);
                        await LoadingWorkshopPage(1);
                    }
                }
            });
        }

        private PagerInfoDto pagerInfoDto;
        public async Task LoadingWorkshopPage(int pageNumber = 1, int pageSize = 9)
        {
            try
            {
                var (courses, pager) = await courseApiClient.GetCreatedCoursesAsync(pageNumber, pageSize);

                pagerInfoDto = pager;

                var courseViewModel = courses.Courses
                    .Select(x => new CreateCoursePanelElementVm(x))
                    .ToList();

                GetCourses = courseViewModel;

                VisibleAddButton = authService.CurrentUser?.Role?.Name == "Admin" && pager.TotalItems > 0
                    ? Visibility.Visible
                    : Visibility.Hidden;

                VisibleEmptyPage = pager.TotalItems <= 0
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                VisibleButtonPanel = pager.TotalItems > pager.PageSize
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                HasPreviousPage = pageNumber > 1;
                HasNextPage = pageNumber < GetTotalPages(pager);

                GenerateButtonPanel(pager);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading workshop: {ex.Message}");
            }
        }

        private void UpdateStatistics()
        {
            if (GetCourses == null) return;

            TotalCourses = pagerInfoDto.TotalItems;
            DraftCourses = GetCourses.Count(c => c.Status == "В разработке");
            PublishedCourses = GetCourses.Count(c => c.Status == "Опубликован");
        }


        private int GetTotalPages(PagerInfoDto? pager = null)
        {
            if (pager != null)
            {
                return pager.TotalPages;
            }
            return 1;
        }

        private void GenerateButtonPanel(PagerInfoDto pager)
        {
            var newButtonPanel = pagerService.GeneratePagerPanel(pager, PagerCommand);

            var currentPage = pager.PageNumber;
            foreach (var btn in newButtonPanel)
            {
                if (!btn.IsEllipsis && btn.Text == currentPage.ToString())
                {
                    btn.IsSelected = true;
                }
            }

            ButtonPanel = new ObservableCollection<ButtonItem>(newButtonPanel);
        }
    }
}