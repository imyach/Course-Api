using CourseDesktopClient.Models.DtosModel.Entities;
using CourseDesktopClient.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.UI.Elements.ElementVM.RecoveryPasswordElementsVm
{
    public class SelectionProfileElementVm(UserDto userDto) : ViewModelBase
    {
        public Guid Id => userDto.Id;
        public string NameUser => userDto.NameUser;
        public DateTime CreatedAt => userDto.CreatedAt;
        public RoleDto Role => userDto.Role;
    }
}
