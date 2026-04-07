using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Services;
using CourseDesktopClient.Utilities;
using CourseDesktopClient.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace CourseDesktopClient.ViewModel
{
    public class VideoLogoPageVm : ViewModelBase
    {
        private readonly IAuthService authService;
        private readonly INavigationService navigationService;
        public ICommand EndVideoCommand { get; set; }

        private Uri _sourseVideo;
        public Uri SourseVideo 
        {
            get { return _sourseVideo; }
            set { _sourseVideo = value; OnPropertyChanged(); }
        }

        public VideoLogoPageVm(IAuthService authService, INavigationService navigationService)
        {
            this.authService = authService;
            this.navigationService = navigationService;


            string videoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                "UI", "VideoResouses", "StartedScreen.mp4");

            SourseVideo = new Uri(videoPath);

            EndVideoCommand = new  RelayCommand(async _ => await OnVideoEnded());
        }
        private async Task OnVideoEnded()
        {
            var isAuthenticated = await authService.CheckAuthOnStartupAsync();

            if (isAuthenticated )
                await navigationService.NavigateToCourses();
            else
                navigationService.NavigateToLogin();
        }
    }
}
