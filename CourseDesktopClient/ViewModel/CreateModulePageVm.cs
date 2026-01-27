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
using System.Windows.Media.Media3D;

namespace CourseDesktopClient.ViewModel
{
    public class CreateModulePageVm : NavigationVm
    {
        private IList<CreateMaterialElementVm>? _getMaterial;
        public IList<CreateMaterialElementVm>? GetMaterial { get => _getMaterial; set { _getMaterial = value; OnPropertyChanged(nameof(GetMaterial)); } }

        private string _titleOfModule = string.Empty;
        public string TitleOfModule
        {
            get { return _titleOfModule; }
            set { _titleOfModule = value; OnPropertyChanged(); CheckEnableSave(); }
        }

        private string? _descroptionOfModule;
        public string? DescroptionOfModule
        {
            get { return _descroptionOfModule; }
            set { _descroptionOfModule = value; OnPropertyChanged(); CheckEnableSave(); }
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

        private Visibility _visibleAddMaterialButton;
        public Visibility VisibleAddMaterialButton
        {
            get { return _visibleAddMaterialButton; }
            set
            {
                _visibleAddMaterialButton = value;
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

        public ICommand CreateMaterialCommand { get; set; }
        public ICommand SaveUpdateCommand { get; set; }
        public ICommand DeleteModuleCommand { get; set; }
        public ICommand LocalCreateMaterialCommand { get; set; }
        public ICommand LocalDeleteMaterialCommand { get; set; }
        public ICommand LocalCreateCourseCommand { get; set; }


        public CreateModulePageVm(INavigationService navigationService, ICourseApiClient courseApiClient) : base(navigationService)
        {
            this.courseApiClient = courseApiClient;

            LocalCreateCourseCommand = new RelayCommand(async sender =>
            {
                await navigationService.NavigateToCreateCourse(courseId);
            });

            CreateMaterialCommand = new RelayCommand(async sender =>
            {
                await navigationService.NavigateToCreateMaterial(moduleId);
            });

            SaveUpdateCommand = new RelayCommand(async sender =>
            {
                var updateModule = new ModuleDto
                {
                    Id = moduleId,
                    Title = TitleOfModule,
                    Description = DescroptionOfModule,
                };

                await courseApiClient.UpdateModuleAsync(updateModule);
                CustomMessageBox.ShowInfo("Изменения сохранены");
            });

            LocalCreateMaterialCommand = new RelayCommand(async sender =>
            {
                await navigationService.NavigateToCreateMaterial(moduleId, (sender as CreateMaterialElementVm).Id);
            });

            DeleteModuleCommand = new RelayCommand(async sender =>
            {
                if (CustomMessageBox.ShowYesNo("Вы дествительно хотите удалить данный модуль? \nПосле этого произойдет автоматическое сохранеие!") == DialogResult.Yes)
                {
                    await courseApiClient.DeleteModuleAsync(moduleId);
                    await navigationService.NavigateToCreateCourse(courseId);
                    CustomMessageBox.ShowInfo("Модуль удален");
                }
            });

            LocalDeleteMaterialCommand = new RelayCommand(async sender =>
            {
                if (CustomMessageBox.ShowYesNo("Вы дествительно хотите удалить данный материал? \nПосле этого произойдет автоматическое сохранеие!") == DialogResult.Yes)
                {
                    await courseApiClient.DeleteMaterialAsync((sender as CreateMaterialElementVm).Id);
                    await LoadModule(courseId, moduleId);
                    CustomMessageBox.ShowInfo("Материал удален");
                }
            });
        }

        public static Guid moduleId;
        private Guid courseId;
        public async Task LoadModule(Guid idCourse,Guid idModule)
        {
            courseId = idCourse;
            if (idModule != default)
            {
                moduleId = idModule;
                var module = await courseApiClient.GetModuleAsync(idModule);

                TitleOfModule = module.Title;
                DescroptionOfModule = module.Description;

                var matherials = await courseApiClient.GetMaterialsAsync(idModule);
                var matherialViewModel = matherials.Matherials.Select(x => new CreateMaterialElementVm(x)).ToList();
                GetMaterial = matherialViewModel;

                VisibleAddMaterialButton = GetMaterial.Count > 0
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                VisibleEmptyPage = GetMaterial.Count <= 0
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                CheckEnableSave();
            }
            else
            {
                var newModule = new ModuleDto() { Description = "Описание", Title = "Новый модуль", CourseId = courseId };

                var idNewModule = await courseApiClient.CreateModuleAsync(newModule);

                var module = await courseApiClient.GetModuleAsync(idNewModule ?? throw new Exception());
                moduleId = module.Id;
                DescroptionOfModule = module.Description;
                TitleOfModule = module.Title;



            }
        }

        private void CheckEnableSave()
        {
            if (GetMaterial?.Count > 0 && !string.IsNullOrEmpty(DescroptionOfModule) && !string.IsNullOrEmpty(TitleOfModule))
                IsSaveEnable = true;
            else
                IsSaveEnable = false;
        }
    }
}
