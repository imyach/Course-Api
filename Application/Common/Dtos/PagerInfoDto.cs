using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos
{
    public class PagerInfoDto
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
