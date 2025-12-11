using Application.Common.Dtos.Tests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries.Tests.GetTest
{
    public class GetDetailsTestQuery : IRequest<TestLookupDto>
    {
        public Guid Id { get; set; }

    }
}
