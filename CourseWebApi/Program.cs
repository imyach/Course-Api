using CourseWebApi.Middleware;
using CourseWebApi.Servises;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

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
    services.AddSwaggerGen();
    services.AddScoped<IJwtTokenServise, JwtTokenServise>();
    services.AddScoped<IPasswordHasherServise, PasswordHasherServise>();
}

async Task Configure(WebApplication build)
{
    await app.AddRoleInDataBase();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseCustomExceptionHandler();
    app.UseRouting();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
        app?.MapControllers();
    });
}

