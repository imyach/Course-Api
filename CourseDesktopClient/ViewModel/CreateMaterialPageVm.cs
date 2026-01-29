using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.UI.Elements.ElementVM;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    public class CreateMaterialPageVm : NavigationVm
    {
        private IList<CreateTestElementVm>? _getTest;
        public IList<CreateTestElementVm>? GetTest { get => _getTest; set { _getTest = value; OnPropertyChanged(nameof(GetTest)); } }

        private string _titleOfMaterial = string.Empty;
        public string TitleOfMaterial
        {
            get { return _titleOfMaterial; }
            set { _titleOfMaterial = value; OnPropertyChanged(); CheckEnableSave(); }
        }

        private string? _descroptionOfMaterial;
        public string? DescroptionOfMaterial
        {
            get { return _descroptionOfMaterial; }
            set { _descroptionOfMaterial = value; OnPropertyChanged(); CheckEnableSave(); }
        }

        private bool _isSaveEnable;
        public bool IsSaveEnable
        {
            get { return _isSaveEnable; }
            set
            {
                _isSaveEnable = value;
                OnPropertyChanged();
            }
        }

        private Visibility _visibleAddTestButton;
        public Visibility VisibleAddTestButton
        {
            get { return _visibleAddTestButton; }
            set
            {
                _visibleAddTestButton = value;
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

        public static Guid materialId;
        private Guid moduleId;

        public ICommand CreateTestCommand { get; set; }
        public ICommand SaveUpdateCommand { get; set; }
        public ICommand DeleteMaterialCommand { get; set; }
        public ICommand LocalCreateTestCommand { get; set; }
        public ICommand LocalDeleteTestCommand { get; set; }
        public ICommand LocalTestCommand { get; set; }

        public CreateMaterialPageVm(INavigationService navigationService, ICourseApiClient courseApiClient) : base(navigationService)
        {
            this.courseApiClient = courseApiClient;

            LocalTestCommand = new RelayCommand(async sender =>
            {
                await navigationService.NavigateToCreateModule(CreateCoursePageVm.courseId,moduleId);
            });

            CreateTestCommand = new RelayCommand(async sender =>
            {
                await navigationService.NavigateToCreateTest(materialId);
            });

            LocalCreateTestCommand = new RelayCommand(async sender =>
            {
                await navigationService.NavigateToCreateTest(materialId, (sender as CreateTestElementVm).Id);
            });

            LocalDeleteTestCommand = new RelayCommand(async sender =>
            {
                if (CustomMessageBox.ShowYesNo("Вы дествительно хотите удалить данный тест? \nПосле этого произойдет автоматическое сохранеие!") == DialogResult.Yes)
                {
                    await courseApiClient.DeleteTestAsync((sender as CreateTestElementVm).Id);
                    await LoadMaterial(materialId, moduleId);
                    CustomMessageBox.ShowInfo("Тест удален");
                }
            });

            SaveUpdateCommand = new RelayCommand(async sender =>
            {
                var updateMaterial = new MaterialDto
                {
                    Id = materialId,
                    Title = TitleOfMaterial,
                    Description = DescroptionOfMaterial
                };

                await courseApiClient.UpdateMaterialAsync(updateMaterial);
                CustomMessageBox.ShowInfo("Изменения сохранены");
            });
            DeleteMaterialCommand = new RelayCommand(async sender =>
            {
                if (CustomMessageBox.ShowYesNo("Вы дествительно хотите удалить данный материал? \nПосле этого произойдет автоматическое сохранеие!") == DialogResult.Yes)
                {
                    await courseApiClient.DeleteMaterialAsync(materialId);
                    await navigationService.NavigateToCreateModule(CreateCoursePageVm.courseId, moduleId);
                    CustomMessageBox.ShowInfo("Материал удален");
                }
            });

        }

        public async Task LoadMaterial(Guid idMaterial, Guid idModule)
        {
            moduleId = idModule;
            if (idMaterial != default)
            {
                materialId = idMaterial;
                var material = await courseApiClient.GetMaterialAsync(idMaterial);

                TitleOfMaterial = material.Title;
                DescroptionOfMaterial = material.Description;

                var tests = await courseApiClient.GetTestsAsync(idMaterial);
                var testsViewModel = tests.Tests.Select(x => new CreateTestElementVm(x)).ToList();
                GetTest = testsViewModel;

                VisibleAddTestButton = GetTest.Count > 0
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                VisibleEmptyPage = GetTest.Count <= 0
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                CheckEnableSave();
            }
            else
            {
                var newMaterial = new MaterialDto() { Description = "Содержание", Title = "Новая тема", ModuleId = moduleId };

                var idNewMaterial = await courseApiClient.CreateMaterialAsync(newMaterial);

                var matetial = await courseApiClient.GetMaterialAsync(idNewMaterial ?? throw new Exception());
                materialId = matetial.Id;
                DescroptionOfMaterial = matetial.Description;
                TitleOfMaterial = matetial.Title;

                VisibleEmptyPage = Visibility.Visible;
                VisibleAddTestButton = Visibility.Collapsed;
            }
        }
        private void CheckEnableSave()
        {
            if (GetTest?.Count > 0 && !string.IsNullOrEmpty(TitleOfMaterial))
                IsSaveEnable = true;
            else
                IsSaveEnable = false;
        }
    }
}
