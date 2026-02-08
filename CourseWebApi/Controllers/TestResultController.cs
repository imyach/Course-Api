using Application.Common.Commands.ProgressModules.DeleteProgressModules;
using Application.Common.Commands.ProgressModules.UpdateProgressModules;
using Application.Common.Commands.TestResults.CreateTestResults;
using Application.Common.Commands.TestResults.DeleteTestResults;
using Application.Common.Commands.TestResults.UpdateTestResults;
using Application.Common.Dtos.ProgressModules;
using Application.Common.Dtos.TestResults;
using Application.Common.Queries.ProgressModules.GetProgressModules;
using Application.Common.Queries.ProgressModules.GetProgressModulesList;
using Application.Common.Queries.TestResults.GetTestResults;
using Application.Common.Queries.TestResults.GetTestResultsList;
using AutoMapper;
using CourseWebApi.Models.ProgressModule;
using CourseWebApi.Models.TestResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class TestResultController(IMapper mapper) : BaseController
    {
        [HttpGet("All/{progressMaterialId}")]
        public async Task<ActionResult<TestResultListVm>> GetAll(Guid progressMaterialId)
        {
            var query = new GetAllTestResultQuery
            {
                CurrentUserId = UserId,
                ProgressMaterialId = progressMaterialId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TestResultLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsTestResultQuery
            {
                Id = id,
                CurrentUserId = UserId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteTestResultCommand
            {
                CurrentUserId = UserId,
                Id = id
            };

            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateTestResultDto createTestResultDto)
        {
            var command = mapper.Map<CreateTestResultCommand>(createTestResultDto);
            command.CurrentUserId = UserId;
            var testResultId = await Mediator.Send(command);
            return Ok(testResultId);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTestResultDto updateTestResultDto)
        {
            var command = mapper.Map<UpdateTestResultCommand>(updateTestResultDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }
    }
}