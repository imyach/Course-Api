using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.UI.Elements.ElementVM
{
    public class UserPanelElementVm(UserDto userDto, UserDto currentUser) : ViewModelBase
    {
        public Guid Id => userDto.Id;
        public string NameUser => userDto.NameUser;
        public DateTime CreatedAt => userDto.CreatedAt;
        public RoleDto Role => userDto.Role;

        public bool IsCurrentUser => currentUser?.Id == userDto.Id;
    }
}
