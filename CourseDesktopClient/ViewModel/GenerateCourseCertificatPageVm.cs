using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.Reports;
using CourseDesktopClient.Models.DtosModel.Reports.Course;
using CourseDesktopClient.Utilities;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;

namespace CourseDesktopClient.ViewModel
{
    public class GenerateCourseCertificatPageVm : NavigationVm  
    {
        private string _userFullName = string.Empty;
        public string UserFullName
        {
            get { return _userFullName; }
            set { _userFullName = value; OnPropertyChanged(); }
        }

        private string _savePath = string.Empty;
        public string SavePath
        {
            get { return _savePath; }
            set { _savePath = value; OnPropertyChanged(); }
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
        public ICommand DownloadCertificateCommand { get; set; }
        public ICommand BrowseFolderCommand { get; set; }

        string titleCourse;
        DateTime? passedDate;

        public GenerateCourseCertificatPageVm(INavigationService navigationService, ICourseApiClient courseApiClient) : base(navigationService)
        {
            BrowseFolderCommand = new RelayCommand(async _ =>
            {
                ExecuteBrowseFolder();
            });

            DownloadCertificateCommand = new RelayCommand(async _ =>
            {
                if (!FillingVerification())
                    return;

                var requestDto = new CourseCertificateDto
                {
                    Name = UserFullName,
                    PassedDate = passedDate,
                    CourseTitle = titleCourse,
                };

                var response = await courseApiClient.GenerateCourseCertificate(requestDto);

                if (response?.FileContent == null || response.FileContent.Length == 0)
                {
                    MisstakeText = "Ошибка при формировании сертификата";
                    VisibleMisstake = Visibility.Visible;
                    return;
                }
                await SaveCertificateAsync(response);

                await navigationService.NavigateToMyCourses();
            });
        }
        private bool FillingVerification()
        {
            MisstakeText = string.Empty;
            VisibleMisstake = Visibility.Collapsed;

            if (string.IsNullOrEmpty(UserFullName))
            {
                MisstakeText = "ФИО не может быть пустым";
                VisibleMisstake = Visibility.Visible;
                return false;
            }
            if (UserFullName.Length > 30)
            {
                MisstakeText = "ФИО не может более 30 символов";
                VisibleMisstake = Visibility.Visible;
                return false;
            }

            if (string.IsNullOrEmpty(SavePath))
            {
                MisstakeText = "Заполните путь сохранения";
                VisibleMisstake = Visibility.Visible;
                return false;
            }

            return true;
        }
        public async  Task LoadPage(string titleCourse, DateTime? passedDate)
        {
            this.titleCourse = titleCourse;
            this.passedDate = passedDate;
        }

        private async Task SaveCertificateAsync(CourseCertificateResultDto result)
        {
            string fileName = result.FileName + ".pdf";
            string fullPath = Path.Combine(SavePath, fileName);

            await File.WriteAllBytesAsync(fullPath, result.FileContent);

            CustomMessageBox.ShowInfo($"Сертификат сохранен в файле:\n{fullPath}");
        }

        private void ExecuteBrowseFolder()
        {
            var dialog = new VistaFolderBrowserDialog();
            dialog.Description = "Выберите папку";
            dialog.ShowNewFolderButton = true;
            dialog.SelectedPath = SavePath;

            if (dialog.ShowDialog() == true)
            {
                SavePath = dialog.SelectedPath;
            }
        }
    }
}
