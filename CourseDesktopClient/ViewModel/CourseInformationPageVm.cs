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

            GoBackCommand = new RelayCommand(execute: _ => navigationService.GoBack(),
                                            canExecute: _ => navigationService.CanGoBack);

            PagerCommand = new RelayCommand(async pageNumberStr =>
            {
                if (int.TryParse((pageNumberStr as ButtonItem).Text, out int pageNumber))
                {
                    Update(idCourse, pageNumber);

                }
            });

            RatingCommand = new RelayCommand(async button =>
            {
                raitReview = Convert.ToInt32(button);
            });

            SendReviewCommand = new RelayCommand(async _ =>
            {
                if (raitReview != 0 && !string.IsNullOrEmpty(ReviewUserText))
                {

                    var reviewDto = new ReviewDto
                    {
                        CourseId = idCourse,
                        Text = ReviewUserText,
                        Rait = raitReview
                    };


                    await courseApiClient.CreateReviewAsync(reviewDto);

                    ResetReviewForm();

                    await LoadCourse(idCourse);
                }
            });

            DeleteReviewCommand = new RelayCommand(async button => 
            {
                if (MessageBox.Show("Вы действительно хотите удавлить отзыв?", "Предупреждение!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    var idReview = (button as ReviewPanelElementVm).Id;

                    await courseApiClient.DeleteReviewAsync(idReview);

                    await LoadCourse(idCourse);
                }
            }
            );
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

        public bool IsCurrentUserReview { get; set; } = true;

        private bool _shouldResetRating;
        public bool ShouldResetRating
        {
            get => _shouldResetRating;
            set => SetProperty(ref _shouldResetRating, value);
        }
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

        private readonly ICourseApiClient courseApiClient;
        private readonly IPagerService pagerService;
        private readonly IAuthService authService;

        public ICommand GoBackCommand { get; set; }
        public ICommand DeleteReviewCommand { get; set; }
        public ICommand SendReviewCommand { get; set; }
        public ICommand PagerCommand { get; set; }
        public ICommand RatingCommand { get; set; }

        public async Task LoadCourse(Guid id)
        {
            idCourse = id;
            Update(idCourse);
        }

        public async void Update(Guid id, int pageNumber = 1)
        {
            var infoCourse = await courseApiClient.GetCourseByIdAsync(id);
            var (reviews, pager) = await courseApiClient.GetReviewsAsync(id, pageNumber);

            Title = infoCourse.Title;
            Description = infoCourse.Description;
            CreatedAt = infoCourse.CreatedAt;



            var reviewViewModels = reviews.Reviews.Select(x=> new ReviewPanelElementVm(x, authService.CurrentUser.Id))
                .ToList();

            GetReviews = reviewViewModels;
            GenerateButtonPanel(pager);
        }
        private void GenerateButtonPanel(PagerInfoDto pager)
        {
            var newButtonPanel = pagerService.GeneratePagerPanel(pager, PagerCommand);
            ButtonPanel = new ObservableCollection<ButtonItem>(newButtonPanel);
        }
    }
}
