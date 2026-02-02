using Application.Common.Dtos.ProgressMaterials;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.ProgressModules
{
    public class ProgressModuleListVm
    {
        public IList<ProgressModuleLookupDto> ProgressModule { get; set; } = [];
    }
}
