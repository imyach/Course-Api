using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Matherials
{
    public class MatherialListVm
    {
        public IList<MatherialLookupDto> Matherials { get; set; } = [];
    }
}
