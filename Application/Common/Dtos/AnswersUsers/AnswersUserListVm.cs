using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.AnswersUsers
{
    public class AnswersUserListVm
    {
        public IList<AnswersUserLookupDto> AnswersUsers { get; set; } = [];
    }
}
