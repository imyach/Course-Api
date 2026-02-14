using Application.Common.Dtos.Answers;
using Application.Common.Dtos.AnswersUsers;
using Application.Common.Exceptions;
using Application.Common.Queries.Answers.GetAnswerList;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static Application.Common.Dtos.AnswersUsers.TestResult.CheckingResponsesDto;

namespace Application.Common.Queries.AnswersUsers.GetAnswersUserList
{
    public class GetAllAnswersUserQueryHandler : IRequestHandler<GetAllAnswersUserQuery, TestHistoryVm>
    {
        private readonly ICoursesDbContext _context;

        public GetAllAnswersUserQueryHandler(ICoursesDbContext context)
        {
            _context = context;
        }

        public async Task<TestHistoryVm> Handle(GetAllAnswersUserQuery request, CancellationToken cancellationToken)
        {
            // 1. Сначала находим все TestResult для этого теста и пользователя
            var testResults = await _context.TestResults
                .Include(tr => tr.Test)
                .Where(tr => tr.TestId == request.TestId && tr.UserId == request.CurrentUserId)
                .OrderByDescending(tr => tr.CompletedAt)
                .Select(tr => new TestAttemptDto
                {
                    Id = tr.Id,
                    TestId = tr.TestId,
                    TestTitle = tr.Test != null ? tr.Test.Title : "Тест",
                    Score = tr.Score,
                    IsPassed = tr.IsPassed,
                    CompletedAt = tr.CompletedAt,
                })
                .ToListAsync(cancellationToken);

            // 2. Получаем информацию о тесте
            var test = await _context.Tests
                .FirstOrDefaultAsync(t => t.Id == request.TestId, cancellationToken);

            return new TestHistoryVm
            {
                TestId = request.TestId,
                TestTitle = test?.Title ?? "Тест",
                TotalQuestions = test?.Questions?.Count() ?? 0,
                Attempts = testResults,
                BestScore = testResults.Any() ? testResults.Max(tr => tr.Score) : 0,
                IsTestPassed = testResults.Any(tr => tr.IsPassed)
            };
        }
    }
}