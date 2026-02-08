using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.UI.Elements.ElementVM;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    public class CompletingModulePageVm : NavigationVm
    {
        private IList<CompliteMaterialElementVm>? _getProgressMaterials;
        public IList<CompliteMaterialElementVm>? GetProgressMaterials { get => _getProgressMaterials; set { _getProgressMaterials = value; OnPropertyChanged(nameof(GetProgressMaterials)); } }

        private string _title = string.Empty;
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }

        private string? _descroption;
        public string? Description
        {
            get { return _descroption; }
            set { _descroption = value; OnPropertyChanged(); }
        }

        private readonly ICourseApiClient courseApiClient;
        public static Guid localIdProgressCourse;
        public static Guid localIdProgressModule;
        public ICommand PassMaterialCommand { get; set; }
        public ICommand PassCourseCommand { get; set; }

        public CompletingModulePageVm(INavigationService navigationService, ICourseApiClient courseApiClient) : base(navigationService)
        {
            this.courseApiClient = courseApiClient;

            PassMaterialCommand = new RelayCommand(async sender =>
            {
                await courseApiClient.UpdateProgressMaterialAsync(new ProgressMaterialDto
                {
                    Id = (sender as CompliteMaterialElementVm).Id,
                    Status = "В прохождении",
                    StartedAt = DateTime.UtcNow
                });

                await navigationService.NavigateToProgressMaterial(localIdProgressModule, (sender as CompliteMaterialElementVm).Id);
            });


            PassCourseCommand = new RelayCommand(async sender =>
            {
                await navigationService.NavigateToProgressCourse(localIdProgressCourse);
            });

        }

        public async Task LoadProgressModule(Guid progressCourseId, Guid progressModuleId)
        {
            localIdProgressCourse = progressCourseId;
            localIdProgressModule = progressModuleId;

            var infoProgressModule = await courseApiClient.GetProgressModuleByIdAsync(progressModuleId);

            Title = infoProgressModule.Module.Title;
            Description = infoProgressModule.Module.Description;

            var progressMaterials = await courseApiClient.GetProgressMaterialsAsync(progressModuleId);
            var progressMaterialViewModel = progressMaterials.ProgressMaterials.Select(x => new CompliteMaterialElementVm(x)).ToList();
            GetProgressMaterials = progressMaterialViewModel;
        }
    }
}
