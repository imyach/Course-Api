using Application.Common.Mappings;
using Application.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Persistance;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Common
{
    public class QueryTestFixture : IDisposable 
    {
        public IMapper Mapper;
        public CoursesDbContext Context;

        public QueryTestFixture()
        {
            Context = CoursesContextFactory.Create();
            var loggerFactory = NullLoggerFactory.Instance; 

            var configProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AssemblyMappingProfile(typeof(ICoursesDbContext).Assembly));
            }, loggerFactory);
            Mapper = configProvider.CreateMapper();
        }

        public void Dispose()
        {
            CoursesContextFactory.Destroy(Context);
        }
    }
    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
}
