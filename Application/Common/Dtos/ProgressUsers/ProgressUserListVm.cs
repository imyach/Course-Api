using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.ProgressUsers
{
    public class ProgressUserListVm
    {
        public IList<ProgressUserLookupDto> ProgressUsers { get; set; } = [];
    }
}
