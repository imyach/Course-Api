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
    public class CompletingMaterialPageVm : NavigationVm
    {
        private IList<TestResultElementVm>? _getTestsResults;
        public IList<TestResultElementVm>? GetTestsResults { get => _getTestsResults; set { _getTestsResults = value; OnPropertyChanged(nameof(GetTestsResults)); } }

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
        public static Guid localIdProgressModule;
        public static Guid localIdProgressMaterial;
        public ICommand PassTestCommand { get; set; }
        public ICommand PassModuleCommand { get; set; }
        public ICommand LookTestResults { get; set; }

        public CompletingMaterialPageVm(INavigationService navigationService, ICourseApiClient courseApiClient) : base(navigationService)
        {
            this.courseApiClient = courseApiClient;

            LookTestResults = new RelayCommand(async sender =>
            {
                await navigationService.NavigateToResultsTest((sender as TestResultElementVm).Id);
            });

            PassTestCommand = new RelayCommand(async sender =>
            {   
                await navigationService.NavigateToTestResult(localIdProgressMaterial, (sender as TestResultElementVm).Id);
            });


            PassModuleCommand = new RelayCommand(async sender =>
            {
                await navigationService.NavigateToProgressModule(CompletingModulePageVm.localIdProgressCourse, localIdProgressModule);
            });
        }

        public async Task LoadProgressMaterial(Guid progressModuleId, Guid progressMaterialId)
        {
            localIdProgressModule = progressModuleId;
            localIdProgressMaterial = progressMaterialId;

            var infoProgressMaterial = await courseApiClient.GetProgressMaterialByIdAsync(progressMaterialId);

            Title = infoProgressMaterial.Material.Title;
            Description = infoProgressMaterial.Material.Description;

            var testResults = await courseApiClient.GetTestResultsAsync(progressMaterialId);
            var testResultViewModel = testResults.TestResults.Select(x => new TestResultElementVm(x)).ToList();
            GetTestsResults = testResultViewModel;
        }   

    }
}
