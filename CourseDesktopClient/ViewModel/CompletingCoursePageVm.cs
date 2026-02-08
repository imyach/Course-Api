using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.UI.Elements.ElementVM;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    class CompletingCoursePageVm : NavigationVm
    {
        private IList<CompliteModuleElementVm>? _getProgressModules;
        public IList<CompliteModuleElementVm>? GetProgressModules { get => _getProgressModules; set { _getProgressModules = value; OnPropertyChanged(nameof(GetProgressModules)); } }

        private string _title = string.Empty;
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged();  }
        }

        private string? _descroption;
        public string? Description
        {
            get { return _descroption; }
            set { _descroption = value; OnPropertyChanged();  }
        }

        private readonly ICourseApiClient courseApiClient;
        public static Guid localIdProgressCourse;
        public ICommand PassModuleCommand { get; set; }


        public CompletingCoursePageVm(INavigationService navigationService, ICourseApiClient courseApiClient) : base(navigationService)
        {
            this.courseApiClient = courseApiClient;

            PassModuleCommand = new RelayCommand(async sender =>
            {
                await courseApiClient.UpdateProgressModuleAsync(new Models.DtosModel.Entities.ProgressModuleDto
                {
                    Id = (sender as CompliteModuleElementVm).Id,
                    Status = "В прохождении",
                    StartedAt = DateTime.UtcNow
                });

                await navigationService.NavigateToProgressModule(localIdProgressCourse, (sender as CompliteModuleElementVm).Id);
            });
        }


        public async Task LoadProgressCourse(Guid idProgressCourse)
        {
            localIdProgressCourse = idProgressCourse;
            var infoProgressCourse = await courseApiClient.GetProgressUserByIdAsync(idProgressCourse);

            Title = infoProgressCourse.Course.Title;
            Description = infoProgressCourse.Course.Description;

            var progressModules = await courseApiClient.GetProgressModulesAsync(idProgressCourse);
            var progressModulesViewModel = progressModules.ProgressModules.Select(x => new CompliteModuleElementVm(x)).ToList();
            GetProgressModules = progressModulesViewModel;

        }

    }
}
