using Application.Common.Commands.Rewies.CreateReview;
using Application.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Commands.ProgressUsers.CreateProgressUser
{
    public class CreateProgressUserCommandHandler(ICoursesDbContext context) : IRequestHandler<CreateProgressUserCommand, Guid>
    {
        public async Task<Guid> Handle(CreateProgressUserCommand request, CancellationToken cancellationToken)
        {

            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var progressUser = new ProgressUser
                {
                    Id = Guid.NewGuid(),
                    CourseId = request.CourseId,
                    UserId = request.CurrentUserId,
                    Status = "В прохождении",
                    StartedAt = DateTime.UtcNow,
                    FineshedAt = null,
                };
                if (context.ProgressUsers.Any(pu => pu.UserId == progressUser.UserId && pu.CourseId == progressUser.CourseId))
                    return Guid.Empty;

                await context.ProgressUsers.AddAsync(progressUser, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                var modules = await context.Modules
                    .Include(m=>m.Materials)
                        .ThenInclude(m=>m.Tests)
                    .Where(m=>m.CourseId == request.CourseId)
                    .OrderBy(m=>m.Order)
                    .ToListAsync(cancellationToken);

                foreach (var module in modules) 
                {
                    var progressModule = new ProgressModule
                    {
                        Id = Guid.NewGuid(),
                        UserId = request.CurrentUserId,
                        ModuleId = module.Id,
                        ProgressUserId = progressUser.Id,
                        Status = "Не начат",
                        StartedAt = null,
                        Order = module.Order
                    };

                    context.ProgressModules.Add(progressModule);
                    await context.SaveChangesAsync(cancellationToken);

                    var materials = module.Materials.OrderBy(m => m.Order).ToList();
                    foreach (var material in materials)
                    {
                        var progressMaterial = new ProgressMaterial
                        {
                            Id = Guid.NewGuid(),
                            UserId = request.CurrentUserId,
                            MaterialId = material.Id,
                            ProgressModuleId = progressModule.Id,
                            Status = "Не начатa",
                            StartedAt = null,
                            Order = material.Order,
                        };
                        context.ProgressMaterials.Add(progressMaterial);
                        await context.SaveChangesAsync(cancellationToken);

                        foreach (var test in material.Tests.OrderBy(t => t.Order) ?? Enumerable.Empty<Test>())
                        {
                            var testResult = new TestResult
                            {
                                Id = Guid.NewGuid(),
                                UserId = request.CurrentUserId,
                                TestId = test.Id,
                                ProgressMaterialId = progressMaterial.Id,
                                Score = 0,
                                IsPassed = false,
                                CompletedAt = null
                            };
                            context.TestResults.Add(testResult);
                        }
                    }
                    await context.SaveChangesAsync(cancellationToken);
                }
                await transaction.CommitAsync(cancellationToken);
                return progressUser.Id;

            }
            catch (Exception ex) 
            {
                await transaction.RollbackAsync(cancellationToken);
                throw new Exception(ex.Message);
            }
        }
    }
}
