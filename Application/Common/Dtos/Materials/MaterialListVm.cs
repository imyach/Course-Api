using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Materials
{
    public class MaterialListVm
    {
        public IList<MaterialLookupDto> Materials { get; set; } = [];
    }
}
