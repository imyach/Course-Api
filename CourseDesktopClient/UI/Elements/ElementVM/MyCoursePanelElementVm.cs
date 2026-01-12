using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.UI.Elements.ElementVM
{
    public class MyCoursePanelElementVm(ProgressUserDto progressUserDto)
    {
        public Guid Id => progressUserDto.Id;
        public Guid CourseId => progressUserDto.Course.Id;
        public string Title => progressUserDto.Course.Title;
        public string Description => progressUserDto.Course.Description;
        public string Status => progressUserDto.Status;
        public string CreatedBy => progressUserDto.User.NameUser;
        public DateTime StartedAt => progressUserDto.StartedAt;
        public DateTime? FinishedAt => progressUserDto.FineshedAt;

    }
}
