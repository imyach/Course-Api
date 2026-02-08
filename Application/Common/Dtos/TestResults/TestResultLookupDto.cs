using Application.Common.Dtos.Modules;
using Application.Common.Dtos.ProgressMaterials;
using Application.Common.Dtos.ProgressModules;
using Application.Common.Dtos.Tests;
using Application.Common.Dtos.Users;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.TestResults
{
    public class TestResultLookupDto : IMapWith<TestResult>
    {
        public Guid Id { get; set; }
        public int Score { get; set; }
        public bool IsPassed { get; set; }
        public DateTime CompletedAt { get; set; }

        public ProgressMaterialLookupDto? ProgressMaterial { get; set; }
        public UserLookupDto? User { get; set; }
        public TestLookupDto? Test { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TestResult, TestResultLookupDto>()
                .ForMember(testResultVm => testResultVm.Id,
                opt => opt.MapFrom(testResult => testResult.Id))
                .ForMember(testResultVm => testResultVm.IsPassed,
                opt => opt.MapFrom(testResult => testResult.IsPassed))
                .ForMember(testResultVm => testResultVm.Score,
                opt => opt.MapFrom(testResult => testResult.Score))
                .ForMember(testResultVm => testResultVm.CompletedAt,
                opt => opt.MapFrom(testResult => testResult.CompletedAt))
                .ForMember(testResultVm => testResultVm.ProgressMaterial,
                opt => opt.MapFrom(testResult => testResult.ProgressMaterial))
                .ForMember(testResultVm => testResultVm.User,
                opt => opt.MapFrom(testResult => testResult.User))
                .ForMember(testResultVm => testResultVm.Test,
                opt => opt.MapFrom(testResult => testResult.Test));
        }
    }
}
