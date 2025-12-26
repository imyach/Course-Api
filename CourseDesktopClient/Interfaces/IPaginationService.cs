using CourseDesktopClient.Models;
using CourseDesktopClient.Models.DtosModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CourseDesktopClient.Interfaces
{
    public interface IPaginationService
    {
        public List<ButtonItem> GeneratePaginationPanel(PagerInfoDto pager, ICommand command);
    }
}
