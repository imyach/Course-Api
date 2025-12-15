using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["DbConnection"];
            services.AddDbContext<CoursesDbContext>(options =>
            {
                options.UseNpgsql (connectionString);
            });
            services.AddScoped<ICoursesDbContext, CoursesDbContext>(provider =>
                provider.GetRequiredService<CoursesDbContext>());
            return services;

        }
    }
}
