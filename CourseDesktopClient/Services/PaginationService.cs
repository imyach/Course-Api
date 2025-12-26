using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Utilities;
using CourseDesktopClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourseDesktopClient.Services
{
    public class PaginationService : IPaginationService
    {

        private List<ButtonItem>? Buttons;

        public List<ButtonItem> GeneratePaginationPanel(PagerInfoDto pager, ICommand command)
        {
            Buttons = [];

            int currentPage = pager.PageNumber;
            int totalPages = pager.TotalPages;


            AddButton(1, currentPage, command);

            if (currentPage - 3 > 1)
            {
                Buttons.Add(new ButtonItem { Text = "...", Command = null, IsEllipsis = true });
            }

            for (int i = Math.Max(2, currentPage - 2); i <= Math.Min(totalPages - 1, currentPage + 2); i++)
            {
                AddButton(i, currentPage, command);
            }

            if (currentPage + 3 < totalPages)
            {
                Buttons.Add(new ButtonItem { Text = "...", Command = null, IsEllipsis = true });
            }

            if (totalPages > 1)
            {
                AddButton(totalPages, currentPage, command);
            }
            return Buttons;
        }

        private void AddButton(int pageNumber, int currentPage, ICommand command)
        {
            Buttons.Add(new ButtonItem
            {
                Command = command,
                Text = $"{pageNumber}",
                IsEllipsis = false
            });
        }
    }
}
