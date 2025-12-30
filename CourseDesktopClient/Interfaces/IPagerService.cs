using CourseDesktopClient.Models;
using CourseDesktopClient.Models.DtosModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CourseDesktopClient.Interfaces
{
    public interface IPagerService
    {
        public List<ButtonItem> GeneratePagerPanel(PagerInfoDto pager, ICommand command);
    }
}
