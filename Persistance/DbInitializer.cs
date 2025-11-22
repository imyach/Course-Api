using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance
{
    public class DbInitializer
    {
        public static async void Initialize(CoursesDbContext context, CancellationToken cancellation)
        {
            await context.Database.EnsureCreatedAsync( cancellation);
        }
    }
}
