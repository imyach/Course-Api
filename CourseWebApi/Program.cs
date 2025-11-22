using CourseWebApi.Servises;

var builder = WebApplication.CreateBuilder(args);
RegisterServices(builder.Services);

var app = builder.Build();
await Configure(app);



app.Run();


void RegisterServices(IServiceCollection services) { 

    services.AddAutoMapper(options =>
    {
        options.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
        options.AddProfile(new AssemblyMappingProfile(typeof(ICoursesDbContext).Assembly));
    });

    services.AddHttpContextAccessor();
    services.AddApplication();
    services.AddPersistance(builder.Configuration);
    services.AddControllers();

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddSingleton<ICurrentUserService, CurrentUserService>();
}

async Task Configure(WebApplication build)
{
    using var scope = app.Services.CreateScope();
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<CoursesDbContext>();
        DbInitializer.Initialize(context, CancellationToken.None);

        foreach (var nameRole in Enum.GetNames<EnumRoles>())
        {
            if (!context.Roles.Any(name => name.RoleName == nameRole))
            {
                await context.Roles.AddAsync(new Role()
                {
                    Id = Guid.NewGuid(),
                    RoleName = nameRole,
                    CreatedAt = DateTime.Now
                });
            }
        }
        await context.SaveChangesAsync();

    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseRouting();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.UseEndpoints(endpoints  =>
    {
        app.MapControllers();
    });
}

