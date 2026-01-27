using Application.Common.Dtos.Tests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Tests.GetTestList
{
    public class GetAllTestQuery : IRequest<TestListVm>
    {
        public Guid MaterialId { get; set; }
    }
}
