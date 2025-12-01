using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Answers
{
    public class AnswerListVm 
    {
        public IList<AnswerLookupDto> Answers { get; set; } = [];
    }
}
