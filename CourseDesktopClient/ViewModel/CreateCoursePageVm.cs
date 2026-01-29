using CourseDesktopClient.Api.Client;
using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Services;
using CourseDesktopClient.UI.Elements.ElementVM;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    public class CreateCoursePageVm : NavigationVm
    {
        private IList<CreateModulePanelElementVm>? _getModules;
        public IList<CreateModulePanelElementVm>? GetModules { get => _getModules; set { _getModules = value; OnPropertyChanged(nameof(GetModules)); } }

        private string _titleOfCourse = string.Empty;
        public string TitleOfCourse
        {
            get { return _titleOfCourse; }
            set { _titleOfCourse = value; OnPropertyChanged(); CheckEnablePublish(); }
        }

        private string? _descroptionOfCourse;
        public string? DescroptionOfCourse
        {
            get { return _descroptionOfCourse; }
            set { _descroptionOfCourse = value; OnPropertyChanged(); CheckEnablePublish(); }
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

        private Visibility _visibleAddModuleButton;
        public Visibility VisibleAddModuleButton
        {
            get { return _visibleAddModuleButton; }
            set
            {
                _visibleAddModuleButton = value;
                OnPropertyChanged();
            }
        }

        private Visibility _visiblePublishButton;
        public Visibility VisiblePublishButton
        {
            get { return _visiblePublishButton; }
            set
            {
                _visiblePublishButton = value;
                OnPropertyChanged();
            }
        }
        private Visibility _visibleUnPublishButton;
        public Visibility VisibleUnPublishButton
        {
            get { return _visibleUnPublishButton; }
            set
            {
                _visibleUnPublishButton = value;
                OnPropertyChanged();
            }
        }

        private bool _isPublishEnable;
        public bool IsPublishEnable
        {
            get { return _isPublishEnable; }
            set
            {
                _isPublishEnable = value;
                OnPropertyChanged();
            }
        }

        public static Guid courseId;

        public ICommand SaveUpdateCommand {  get; set; }
        public ICommand PublicCourseCommand {  get; set; }
        public ICommand UnPublicCourseCommand {  get; set; }
        public ICommand LocalCreateModuleCommand {  get; set; }
        public ICommand DeteleModuleCommand {  get; set; }
        public ICommand CreateModuleCommand {  get; set; }

        private readonly ICourseApiClient courseApiClient;

        public CreateCoursePageVm(INavigationService navigationService, ICourseApiClient courseApiClient) : base(navigationService)
        {
            this.courseApiClient = courseApiClient;

            SaveUpdateCommand = new RelayCommand(async sender =>
            {
                var updateCourse = new CourseDto
                {
                    Id = courseId,
                    Title = TitleOfCourse,
                    Description = DescroptionOfCourse,
                };

                await courseApiClient.UpdateCourseAsync(updateCourse);
                CustomMessageBox.ShowInfo("Изменения сохранены");
            });
            DeteleModuleCommand = new RelayCommand(async sender =>
            {
                if (CustomMessageBox.ShowYesNo("Вы дествительно хотите удалить данный модуль? \nПосле этого произойдет автоматическое сохранеие!") == DialogResult.Yes)
                {
                    await courseApiClient.DeleteModuleAsync((sender as CreateModulePanelElementVm).Id);
                    CustomMessageBox.ShowInfo("Модуль удален");
                    await LoadCourse(courseId);
                }
            });

            PublicCourseCommand = new RelayCommand(async sender =>
            {
                if (CustomMessageBox.ShowYesNo("Опубликовать курс?") == DialogResult.Yes)
                {
                    var updateCourse = new CourseDto
                    {
                        Id = courseId,
                        Title = TitleOfCourse,
                        Description = DescroptionOfCourse,
                        Status = "Published",
                    };

                    await courseApiClient.UpdateCourseAsync(updateCourse);
                    CustomMessageBox.ShowInfo("Курс опубликован");
                }
                await navigationService.NavigateToWorkshop();
            });

            UnPublicCourseCommand = new RelayCommand(async sender =>
            {
                if (CustomMessageBox.ShowYesNo("Снять курс с публикации?") == DialogResult.Yes)
                {
                    var updateCourse = new CourseDto
                    {
                        Id = courseId,
                        Status = "Draft",
                    };

                    await courseApiClient.UpdateCourseAsync(updateCourse);
                    CustomMessageBox.ShowInfo("Курс снят с публикации");
                    await navigationService.NavigateToWorkshop();
                }
            });

            LocalCreateModuleCommand = new RelayCommand(async sender =>
            {
                await navigationService.NavigateToCreateModule(courseId, (sender as CreateModulePanelElementVm).Id);
            });

            CreateModuleCommand = new RelayCommand(async sender =>
            {
                await navigationService.NavigateToCreateModule(courseId);
            });

        }

        public async Task LoadCourse(Guid id)
        {
            if (id != default)
            {
                courseId = id ;
                var course = await courseApiClient.GetCourseByIdAsync(id);

                TitleOfCourse = course.Title;
                DescroptionOfCourse = course.Description;

                var modules = await courseApiClient.GetModulesAsync(id);
                var modulesViewModel = modules.Modules.Select(x => new CreateModulePanelElementVm(x)).ToList();
                GetModules = modulesViewModel;

                VisibleEmptyPage = GetModules.Count <= 0
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                VisibleAddModuleButton = GetModules.Count > 0
                   ? Visibility.Visible
                   : Visibility.Collapsed;

                VisiblePublishButton = course.Status == "Published"
                    ? Visibility.Collapsed
                    : Visibility.Visible;

                VisibleUnPublishButton = course.Status != "Published"
                    ? Visibility.Collapsed 
                    : Visibility.Visible;

                CheckEnablePublish();
            }
            else
            {
                var newCourse = new CourseDto() { Description = "Описание", Title = "Новый курс" };

                var idNewCourse = await courseApiClient.CreateCourseAsync(newCourse);

                var course = await courseApiClient.GetCourseByIdAsync(idNewCourse ?? throw new Exception());
                courseId = course.Id;
                TitleOfCourse = course.Title;
                DescroptionOfCourse = course.Description;

                VisibleEmptyPage = Visibility.Visible;
                VisibleAddModuleButton = Visibility.Collapsed;
                VisiblePublishButton = Visibility.Visible;
                VisibleUnPublishButton = Visibility.Collapsed;
            }
        }

        private void CheckEnablePublish()
        {
            if (GetModules?.Count > 0 && !string.IsNullOrEmpty(TitleOfCourse))
                IsPublishEnable = true;
            else
                IsPublishEnable = false;
        }

    }

}
