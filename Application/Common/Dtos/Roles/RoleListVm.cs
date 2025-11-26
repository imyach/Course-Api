using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Roles
{
    public class RoleListVm
    {
        public IList<RoleLookupDto> Roles { get; set; } = [];
    }
}
