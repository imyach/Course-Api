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
        /// <summary>
        /// Get all tests results by user progress material id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/testResult/all/AB670EFA-9049-46F2-AA3F-8C5044657851
        /// </remarks>
        /// <param name="progressMaterialId">Progress material id guid</param>
        /// <returns>Returns TestResultListVm</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("All/{progressMaterialId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Get info test result by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/testResult/AB670EFA-9049-46F2-AA3F-8C5044657851
        /// </remarks>
        /// <param name="id">Test result id guid</param>
        /// <returns>Returns TestResultLookupDto</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Delete object test result by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE (HOST)/api/testResult/AB670EFA-9049-46F2-A5BF-8C5044287851
        /// </remarks>
        /// <param name="id">Test result id (guid)</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Create object testResult 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST (HOST)/api/testResult
        ///  {
        ///    "progressMaterialId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///    "testId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///  }
        /// </remarks>
        /// <param name="createTestResultDto">CreateTestResultDto object</param>
        /// <returns>Returns id (guid)</returns>
        /// <response code="201">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateTestResultDto createTestResultDto)
        {
            var command = mapper.Map<CreateTestResultCommand>(createTestResultDto);
            command.CurrentUserId = UserId;
            var testResultId = await Mediator.Send(command);
            return Ok(testResultId);
        }

        /// <summary>
        /// Update object test result
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST (HOST)/api/testResult
        ///{
        ///  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///  "score": 0,
        ///  "isPassed": true,
        ///  "complitedAt": "2026-03-02T15:55:45.491Z"
        ///}
        /// </remarks>
        /// <param name="updateTestResultDto">UpdateTestResultDto object</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update([FromBody] UpdateTestResultDto updateTestResultDto)
        {
            var command = mapper.Map<UpdateTestResultCommand>(updateTestResultDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }
    }
}