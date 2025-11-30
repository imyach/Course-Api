using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.Tests
{
    public class TestListVm
    {
        public IList<TestLookupDto> Tests { get; set; } = [];
    }
}
