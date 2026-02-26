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
    public class PagerService : IPagerService
    {
        private List<ButtonItem>? Buttons;

        public List<ButtonItem> GeneratePagerPanel(PagerInfoDto pager, ICommand command)
        {
            Buttons = new List<ButtonItem>();

            int currentPage = pager.PageNumber;
            int totalPages = pager.TotalPages;

            AddButton(1, currentPage, command, isSelected: currentPage == 1);

            if (currentPage - 2 > 1)
            {
                Buttons.Add(new ButtonItem
                {
                    Text = "...",
                    Command = null,
                    IsEllipsis = true,
                    IsSelected = false
                });
            }

            int startPage = Math.Max(2, currentPage - 1);
            int endPage = Math.Min(totalPages - 1, currentPage + 1);

            for (int i = startPage; i <= endPage; i++)
            {
                AddButton(i, currentPage, command, isSelected: currentPage == i);
            }

            if (currentPage + 2 < totalPages)
            {
                Buttons.Add(new ButtonItem
                {
                    Text = "...",
                    Command = null,
                    IsEllipsis = true,
                    IsSelected = false
                });
            }

            if (totalPages > 1)
            {
                AddButton(totalPages, currentPage, command, isSelected: currentPage == totalPages);
            }

            return Buttons;
        }

        private void AddButton(int pageNumber, int currentPage, ICommand command, bool isSelected = false)
        {
            Buttons.Add(new ButtonItem
            {
                Command = command,
                Text = pageNumber.ToString(),
                IsEllipsis = false,
                IsSelected = isSelected
            });
        }
    }
}