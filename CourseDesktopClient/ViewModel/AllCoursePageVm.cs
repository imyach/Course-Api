using CourseDesktopClient.ApiConnection;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using CourseDesktopClient.Utilities;
using CredentialManagement;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourseDesktopClient.ViewModel
{
    public class AllCoursePageVm : NavigationVm
    {
        private IList<CourseDto>? _getCourses;
        public IList<CourseDto>? GetCourses { get => _getCourses; set { _getCourses = value; OnPropertyChanged(); } }

        private ObservableCollection<ButtonItem>? _buttonPanel = [];
        public ObservableCollection<ButtonItem>? ButtonPanel
        {
            get => _buttonPanel;
            set
            {
                _buttonPanel = value;
                OnPropertyChanged(nameof(ButtonPanel));
            }
        }
        


        public ICommand LoadedCommand  => new RelayCommand(async _ => 
        {
            LoadCoursesData();
        });

        public async void LoadCoursesData( int currentPageNumber = 1)
        {

            HttpResponseMessage response = await ClientConfig.Client.GetAsync(ApiPaths.API_GET_ALL_COURSE +$"?pageNumber={currentPageNumber}&pageSize=2");

            if (response.IsSuccessStatusCode)
            {
                var jsondata = await response.Content.ReadAsStringAsync();
                var dataArray = JsonSerializer.Deserialize<JsonElement[]>(jsondata) ?? throw new Exception("data is null");

                var cousesElement = dataArray[0];
                var couses = JsonSerializer.Deserialize<CoursesDto>(cousesElement) ?? throw new Exception("course is null");

                var pagerInfoElement = dataArray[1];
                var pagerInfo = JsonSerializer.Deserialize<PagerInfoDto>(pagerInfoElement) ?? throw new Exception("pager is null");

                GetCourses = couses.Courses;

                GeneratePaginationPanel(pagerInfo);
            }
        }


        public void GeneratePaginationPanel(PagerInfoDto pager)
        {
            ButtonPanel.Clear();

            int currentPage = pager.PageNumber;
            int totalPages = pager.TotalPages;


            AddButton(1, currentPage);

            if (currentPage - 3 > 1)
            {
                ButtonPanel.Add(new ButtonItem { Text = "...", Command = null, IsEllipsis = true });
            }

            for (int i = Math.Max(2, currentPage - 2); i <= Math.Min(totalPages - 1, currentPage + 2); i++)
            {
                AddButton(i, currentPage);
            }

            if (currentPage + 3 < totalPages)
            {
                ButtonPanel.Add(new ButtonItem { Text = "...", Command = null, IsEllipsis = true });
            }

            if (totalPages > 1)
            {
                AddButton(totalPages, currentPage);
            }

        }

        private void AddButton(int pageNumber , int currentPage)
        {
            ButtonPanel.Add(new ButtonItem 
            { 
                Command = PaginationCommand, 
                Text = $"{pageNumber}",
                IsEllipsis = false
            });
        }

        public ICommand PaginationCommand =>  new RelayCommand(pageNumberStr =>
            {
            if (int.TryParse((pageNumberStr as ButtonItem).Text, out int pageNumber))
            {
                LoadCoursesData(pageNumber);
            }
        });
    }
    public class ButtonItem
    {
        public string Text { get; set; }
        public ICommand Command { get; set; }
        public bool IsEllipsis { get; set; }
    }
}
