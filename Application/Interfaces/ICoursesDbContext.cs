using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface ICoursesDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        DbSet<ProgressUser> ProgressUsers { get; set; }
        DbSet<ProgressModule> ProgressModules { get; set; }
        DbSet<ProgressMaterial> ProgressMaterials { get; set; }
        DbSet<Review> Reviews { get; set; }
        DbSet<Course> Courses { get; set; }
        DbSet<Module> Modules { get; set; }
        DbSet<Material> Materials { get; set; }
        DbSet<Test> Tests { get; set; }
        DbSet<TestResult> TestResults { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<Answer> Answers { get; set; }
        DbSet<AnswersUser> AnswersUsers { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
