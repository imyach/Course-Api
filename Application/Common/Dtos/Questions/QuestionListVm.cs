using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Questions
{
    public class QuestionListVm 
    {
        public IList<QuestionLookupDto> Questions { get; set; } = [];
    }
}
