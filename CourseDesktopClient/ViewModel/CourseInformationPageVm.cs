using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.Entities.RequestDto;
using CourseDesktopClient.Services;
using CourseDesktopClient.UI.Elements.ElementVM;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace CourseDesktopClient.ViewModel
{
    class CourseInformationPageVm : NavigationVm
    {
        public CourseInformationPageVm(INavigationService navigationService, ICourseApiClient courseApiClient, IPagerService pagerService, IAuthService authService) : base(navigationService)
        {
            this.courseApiClient = courseApiClient;
            this.pagerService = pagerService;
            this.authService = authService;

            PagerCommand = new RelayCommand(async pageNumberStr =>
            {
                if (int.TryParse((pageNumberStr as ButtonItem).Text, out int pageNumber))
                {
                    Update(pageNumber);

                }
            });

            RatingCommand = new RelayCommand(async button =>
            {
                raitReview = Convert.ToInt32(button);
            });

            SendReviewCommand = new RelayCommand(async _ =>
            {
                    var reviewDto = new ReviewDto
                    {
                        CourseId = idCourse,
                        Text = ReviewUserText,
                        Rait = raitReview
                    };

                if (!FillingVerification(reviewDto))
                    return;

                    await courseApiClient.CreateReviewAsync(reviewDto);

                    ResetReviewForm();

                    await LoadCourse(idCourse);
                
            });

            DeleteReviewCommand = new RelayCommand(async button => 
            {
                if (CustomMessageBox.ShowYesNo("Вы действительно хотите удавлить отзыв?") == DialogResult.Yes)
                {

                    var idReview = (button as ReviewPanelElementVm).Id;

                    await courseApiClient.DeleteReviewAsync(idReview);

                    await LoadCourse(idCourse);
                }
            });

            EnrollCommand = new RelayCommand(async sender =>
            {
                var request = new ProgressUserRequestDto
                {
                    CourseId = idCourse,
                };
                var id = await courseApiClient.CreateProgressUserAsync(request);
                    CustomMessageBox.Show($"Вы записаны на курс {Title}");
                Update();
            });
        }



        private async void ResetReviewForm()
        {

            ReviewUserText = string.Empty;
            raitReview = 0;

            ShouldResetRating = false;
            OnPropertyChanged(nameof(ShouldResetRating));

            await Task.Delay(10);

            ShouldResetRating = true;
            OnPropertyChanged(nameof(ShouldResetRating));
        }

        private int raitReview;
        private Guid idCourse;

        private Visibility _visibleSendReview { get; set; }
        public Visibility VisibleSendReview
        {
            get => _visibleSendReview;
            set  { _visibleSendReview = value; OnPropertyChanged(); }
        }
        private Visibility _visibleDescriptionBorder { get; set; }
        public Visibility VisibleDescriptionBorder
        {
            get => _visibleDescriptionBorder;
            set { _visibleDescriptionBorder = value; OnPropertyChanged(); }
        }
        public bool IsCurrentUserReview { get; set; } = true;

        private bool _shouldResetRating;
        public bool ShouldResetRating
        {
            get => _shouldResetRating;
            set => SetProperty(ref _shouldResetRating, value);
        }
        private string _misstakeText = string.Empty;
        public string MisstakeText
        {
            get { return _misstakeText; }
            set { _misstakeText = value; OnPropertyChanged(); }
        }

        private Visibility? _visibleMisstake = Visibility.Collapsed;
        public Visibility? VisibleMisstake
        {

            get { return _visibleMisstake; }
            set { _visibleMisstake = value; OnPropertyChanged(); }
        }
        private bool _sortByDate = true;
        public bool SortByDate
        {
            get => _sortByDate;
            set
            {
                if (SetProperty(ref _sortByDate, value) && value)
                {
                    SortByRating = false;
                    Update();
                }
            }
        }

        private bool _sortByRating;
        public bool SortByRating
        {
            get => _sortByRating;
            set
            {
                if (SetProperty(ref _sortByRating, value) && value)
                {
                    SortByDate = false;
                    Update();
                }
            }
        }

        private bool _sortAscending = false;
        public bool SortAscending
        {
            get => _sortAscending;
            set
            {
                if (SetProperty(ref _sortAscending, value))
                {
                    OnPropertyChanged(nameof(SortDirectionSymbol));
                    Update();
                }
            }
        }
        private bool _isEnrolled = false;
        public bool IsEnrolled
        {
            get => _isEnrolled;
            set => SetProperty(ref _isEnrolled, value);
        }

        public string SortDirectionSymbol => SortAscending ? "↑" : "↓";

        private string _reviewUserText;
        public string ReviewUserText { get { return _reviewUserText; } set { _reviewUserText = value; OnPropertyChanged(); } }
        private string _description;
        public string Description {get { return _description; } set { _description = value; OnPropertyChanged();}}

        private string _title;
        public string Title{get { return _title; }set { _title = value;OnPropertyChanged();}}

        private DateTime _createdAt;
        public DateTime CreatedAt{get { return _createdAt; }set { _createdAt = value; OnPropertyChanged();} }

        private IList<ReviewPanelElementVm>? _getReviews;
        public IList<ReviewPanelElementVm>? GetReviews { get => _getReviews; set { _getReviews = value; OnPropertyChanged(nameof(GetReviews)); } }
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

        private readonly ICourseApiClient courseApiClient;
        private readonly IPagerService pagerService;
        private readonly IAuthService authService;

        public ICommand DeleteReviewCommand { get; set; }
        public ICommand SendReviewCommand { get; set; }
        public ICommand PagerCommand { get; set; }
        public ICommand RatingCommand { get; set; }
        public ICommand EnrollCommand { get; set; }

        public async Task LoadCourse(Guid id)
        {
            idCourse = id;
            Update();
        }

        public async void Update(int pageNumber = 1)
        {
            var sortBy = ApplySorting();

            var infoCourse = await courseApiClient.GetCourseByIdAsync(idCourse);
            var (reviews, pager) = await courseApiClient.GetReviewsAsync(idCourse,sortBy, SortAscending,pageNumber);

            Title = infoCourse.Title;
            Description = infoCourse.Description;
            CreatedAt = infoCourse.CreatedAt;

            var (progress, _, _) = await courseApiClient.GetProgressUsersAsync(authService.CurrentUser.Id, pageSize: int.MaxValue);
            if (progress != null && authService != null)
                IsEnrolled = progress.ProgressUsers?.Any(pu => pu.Course.Id == idCourse && pu.User.Id == authService.CurrentUser.Id) ?? false;

            if (IsEnrolled)
                VisibleSendReview = Visibility.Visible;
            else VisibleSendReview = 
                    Visibility.Collapsed;

            var reviewViewModels = reviews.Reviews.Select(x=> new ReviewPanelElementVm(x, authService.CurrentUser))
                .ToList();

            GetReviews = reviewViewModels;

            VisibleButtonPanel = pager.TotalItems <= pager.PageSize
               ? Visibility.Collapsed
               : Visibility.Visible;

            VisibleDescriptionBorder = string.IsNullOrEmpty(Description)
               ? Visibility.Collapsed
               : Visibility.Visible;

            GenerateButtonPanel(pager);
        }

        private void GenerateButtonPanel(PagerInfoDto pager)
        {
            var newButtonPanel = pagerService.GeneratePagerPanel(pager, PagerCommand);
            ButtonPanel = new ObservableCollection<ButtonItem>(newButtonPanel);
        }


        private bool FillingVerification(ReviewDto reviewDto)
        {
            if (reviewDto.Rait == 0)
            {
                MisstakeText = "Поставьте оценку";
                VisibleMisstake = Visibility.Visible;
                return false;
            }

            if (string.IsNullOrEmpty(reviewDto.Text))
            {
                MisstakeText = "Напишите текст отзыва";
                VisibleMisstake = Visibility.Visible;
                return false;
            }

            if (reviewDto.Text.Length >3000)
            {
                MisstakeText = "Отзыв не может превышать 3000 символов";
                VisibleMisstake = Visibility.Visible;
                return false;
            }

            MisstakeText = string.Empty;
            VisibleMisstake = Visibility.Collapsed;
            return true;
        }
        private SortEnum ApplySorting()
        {
            if (SortByDate)
            {
                return SortEnum.ByDate;
            }
            return SortEnum.ByRait;
        }
    }
    public enum SortEnum
    {
        ByDate,
        ByRait
    }
}
