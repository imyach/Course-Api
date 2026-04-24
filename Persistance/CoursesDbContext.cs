using Application.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Persistance.EntityFrameworkConfiguration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Persistance
{
    public class CoursesDbContext (DbContextOptions<CoursesDbContext> options) : DbContext (options), ICoursesDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ProgressUser> ProgressUsers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<AnswersUser> AnswersUsers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ProgressMaterial> ProgressMaterials { get; set; }
        public DbSet<ProgressModule> ProgressModules { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<RecoveryCode> RecoveryCode { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new AnswerConfiguration());
            builder.ApplyConfiguration(new AnswersUserConfiguration());
            builder.ApplyConfiguration(new CourseConfiguration());
            builder.ApplyConfiguration(new MaterialConfiguration());
            builder.ApplyConfiguration(new ModuleConfiguration());
            builder.ApplyConfiguration(new ProgressUserConfiguration());
            builder.ApplyConfiguration(new QuestionsConfiguration());
            builder.ApplyConfiguration(new ReviewsConfiguration());
            builder.ApplyConfiguration(new TestConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new RefreshTokenConfiguration());
            builder.ApplyConfiguration(new MaterialConfiguration());
            builder.ApplyConfiguration(new ModuleConfiguration());
            builder.ApplyConfiguration(new TestResultConfiguration());
            builder.ApplyConfiguration(new RecoveryCodeConfiguration());
            base.OnModelCreating(builder);
        }
    }
}
