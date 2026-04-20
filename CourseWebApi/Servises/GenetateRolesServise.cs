namespace CourseWebApi.Servises
{
    public static class GenetateRolesServise
    {
        public static async Task AddRoleInDataBase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            try
            {
                var context = scope.ServiceProvider.GetRequiredService<CoursesDbContext>();
                await DbInitializer.Initialize(context, CancellationToken.None);

                foreach (var nameRole in Enum.GetNames<EnumRoles>())
                {
                    if (!context.Roles.Any(name => name.Name == nameRole))
                    {
                        await context.Roles.AddAsync(new Role()
                        {
                            Id = Guid.NewGuid(),
                            Name = nameRole,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }
                await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
