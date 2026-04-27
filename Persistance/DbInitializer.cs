using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance
{
    public class DbInitializer
    {
        public static async Task Initialize(CoursesDbContext context, CancellationToken cancellation)
        {
            await context.Database.MigrateAsync( cancellation);
        }
    }
}
