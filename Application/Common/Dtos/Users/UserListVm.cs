using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Users
{
    public class UsersListVm
    {
        public IList<UserLookupDto> Users { get; set; } = [];
    }
}
