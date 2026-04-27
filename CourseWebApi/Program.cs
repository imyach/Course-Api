using CourseWebApi.Middleware;
using CourseWebApi.Servises;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.Console()
    .WriteTo.File("Logs/CourseWebApi-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30) 
    .CreateLogger();
try
{
    Log.Information("Starting web application");
    var builder = WebApplication.CreateBuilder(args);
RegisterServices(builder.Services);

var app = builder.Build();
await Configure(app);


    Log.Information("Application started successfully");
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

    services.AddSwaggerGen(config =>
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        config.IncludeXmlComments(xmlPath);
    });

    services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin();
        });
    });

    services.AddAuthentication(cnf =>
    {
        cnf.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        cnf.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer("Bearer", options =>
        {
            options.Audience = "CourseWebApi";
            options.RequireHttpsMetadata = false;


            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = true,
                ValidAudience = "CourseWebApi",
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["SECRET_KEY"])),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };
        });



    services.AddEndpointsApiExplorer();
    services.AddScoped<IJwtTokenServise, JwtTokenServise>();
    services.AddScoped<IEmailServise, EmailServise>();
    services.AddScoped<IHasherServise, HasherServise>();
    services.AddScoped<ICurrentUserService, CurrentUserService>();
    services.AddScoped<IGenerateRandomValueService, GenerateRandomValueService>();

}

async Task Configure(WebApplication app)
{
    await app.AddRoleInDataBase();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(config =>
        {
            config.RoutePrefix = string.Empty;
            config.SwaggerEndpoint("swagger/v1/swagger.json", "v1");
        });
    }
    app.UseCustomExceptionHandler();
    app.UseRouting();
    app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
        endpoints?.MapControllers();
    });
}
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
