using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.Reports;
using CourseDesktopClient.Utilities;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    public class GerenateReportUserPageVm : NavigationVm 
    {
        private string _reportName = string.Empty;
        public string ReportName
        {
            get { return _reportName; }
            set { _reportName = value; OnPropertyChanged(); }
        }

        private string _savePath = string.Empty;
        public string SavePath
        {
            get { return _savePath; }
            set { _savePath = value; OnPropertyChanged(); }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; OnPropertyChanged(); }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; OnPropertyChanged(); }
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
        private bool _isAllTime = false;
        public bool IsAllTime
        {
            get { return _isAllTime; }
            set
            {
                _isAllTime = value;
                OnPropertyChanged();
            }
        }

        private RoleDto _selectedRole;
        public RoleDto SelectedRole
        {
            get { return _selectedRole; }
            set
            {
                _selectedRole = value;
                OnPropertyChanged();
            }
        }

        private string _selectedFormat;
        public string SelectedFormat
        {
            get { return _selectedFormat; }
            set
            {
                _selectedFormat = value;
                OnPropertyChanged();
            }
        }

        private IList<RoleDto>? _getRoles;
        public IList<RoleDto>? GetRoles { get => _getRoles; set { _getRoles = value; OnPropertyChanged(); } }

        private IList<string>? _getFormat;
        public IList<string>? GetFormat { get => _getFormat; set { _getFormat = value; OnPropertyChanged(); } }

        public ICommand DownloadReportCommand { get; set; }
        public ICommand BrowseFolderCommand { get; set; }
        public ICommand LoadedPageCommand { get; set; }
        public GerenateReportUserPageVm(INavigationService navigationService, ICourseApiClient courseApiClient) : base(navigationService)
        {

            BrowseFolderCommand = new RelayCommand(async _ =>
            {
                ExecuteBrowseFolder();
            });

            DownloadReportCommand = new RelayCommand(async _ =>
            {
                if (!FillingVerification())
                    return;

                var requestDto = new UserReportDto
                {
                    Name = ReportName,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    IsAllTime = IsAllTime,
                    Format = SelectedFormat.ToString(),
                    RoleId = SelectedRole.Id == Guid.Empty ? null : SelectedRole.Id,
                };

                var response =  await courseApiClient.GenerateUsersReport(requestDto);
                
                if (response?.FileContent == null || response.FileContent.Length == 0)
                {
                    MisstakeText = "Ошибка при формировании отчета";
                    VisibleMisstake = Visibility.Visible;
                    return;
                }
                await SaveReportAsync(response);

                await navigationService.NavigateToUsers();
            });

            LoadedPageCommand = new RelayCommand(async _ =>
            {
                StartDate = DateTime.UtcNow;
                EndDate = DateTime.UtcNow;
                SavePath = string.Empty;
                ReportName = string.Empty;

                GetFormat = [".xlsx", ".pdf"];

                var roles = (await courseApiClient.GetRolesAsync()).Roles;
                roles.Insert(0, new RoleDto { Name = "Все роли" });

                GetRoles = roles;

                IsAllTime = false;
            });
        }
            


        private async Task SaveReportAsync(UserReportReusltDto result)
        {
            string fileName = result.FileName + SelectedFormat;
            string fullPath = Path.Combine(SavePath, fileName);

            await File.WriteAllBytesAsync(fullPath, result.FileContent);

            CustomMessageBox.ShowInfo($"Отчет сохранен в файле:\n{fullPath}");
        }

        private bool FillingVerification()
        {
            MisstakeText = string.Empty;
            VisibleMisstake = Visibility.Collapsed;

            if (string.IsNullOrEmpty(ReportName))
            {
                MisstakeText = "Заполните название отчета";
                VisibleMisstake = Visibility.Visible;
                return false;
            }

            if (string.IsNullOrEmpty(SavePath))
            {
                MisstakeText = "Заполните путь сохранения";
                VisibleMisstake = Visibility.Visible;
                return false;
            }

            if (EndDate < StartDate && IsAllTime == false)
            {
                MisstakeText = "Конечная дата не может быть раньше начальной";
                VisibleMisstake = Visibility.Visible;
                return false;
            }

            return true;
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
