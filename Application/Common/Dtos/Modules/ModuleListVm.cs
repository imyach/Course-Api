using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Modules
{
    public class ModuleListVm 
    {
        public IList<ModuleLookupDto> Modules { get; set; } = [];
    }
}
