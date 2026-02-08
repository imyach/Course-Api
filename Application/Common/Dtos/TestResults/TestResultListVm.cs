using Application.Common.Dtos.ProgressMaterials;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.TestResults
{
    public class TestResultListVm
    {
        public IList<TestResultLookupDto> TestResults { get; set; } = [];
    }
}
