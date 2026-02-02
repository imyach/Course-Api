using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.ProgressMaterials
{
    public class ProgressMaterialListVm
    {
        public IList<ProgressMaterialLookupDto> ProgressMaterial {  get; set; } = [];
    }
}
