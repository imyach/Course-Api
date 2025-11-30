using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Reviews
{
    public class ReviewListVm
    {
        public IList<ReviewLookupDto> Reviews { get; set; } = [];
    }
}
