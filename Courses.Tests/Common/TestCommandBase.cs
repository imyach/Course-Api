using Application.Common.Commands.AnswersUsers.CreateAnswersUser;
using Application.Interfaces;
using CourseWebApi.Servises;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Persistance;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Tests.Common
{
    public abstract class TestCommandBase : IDisposable
    {
        protected readonly CoursesDbContext Context;
        protected readonly IHasherServise Hasher;
        protected readonly IEmailServise EmailServise;
        protected readonly IJwtTokenServise TokenServise;
        protected readonly ILogger? Logger;
        
        public TestCommandBase()
        {
            Context = CoursesContextFactory.Create();
            EmailServise = new EmailServise();
            Hasher = new HasherServise();

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["SECRET_KEY"] = "sceret key for created jwtToken = dkvcnsdjovas[d4328y24374hg2ui4 n2394y 3287432fsdgjdsfugsdfufhsdjhfbsdjfb"
            })
            .Build();

            TokenServise = new JwtTokenServise(Context, configuration);
            Logger = NullLogger<CreateAnswersUserCommandHandler>.Instance;
        }

        public void Dispose()
        {
            CoursesContextFactory.Destroy(Context);
        }
    }
}
