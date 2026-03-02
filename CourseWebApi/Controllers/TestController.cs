using Application.Common.Commands.Courses.CreateCourse;
using Application.Common.Commands.Courses.DeleteCourse;
using Application.Common.Commands.Courses.UpdateCourse;
using Application.Common.Commands.Tests.CreateTest;
using Application.Common.Commands.Tests.DeleteTest;
using Application.Common.Commands.Tests.UpdateTest;
using Application.Common.Dtos.Courses;
using Application.Common.Dtos.Tests;
using Application.Common.Queries.Courses.GetCourse;
using Application.Common.Queries.Courses.GetCourseList;
using Application.Common.Queries.Tests.GetTest;
using Application.Common.Queries.Tests.GetTestList;
using AutoMapper;
using CourseWebApi.Models.Course;
using CourseWebApi.Models.Tests;
using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    public class TestController(IMapper mapper) : BaseController
    {
        /// <summary>
        /// Get all tests in matherial by matherial id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/test/all/AB670EFA-9049-46F2-A5BF-8C5044287851
        /// </remarks>
        /// <param name="materialId">Material id guid</param>
        /// <returns>Returns TestListVm</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("All/{materialId}")]
        [Authorize(Roles = "Couch,Student,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TestListVm>> GetAll(Guid materialId)
        {
            var query = new GetAllTestQuery()
            {
                MaterialId = materialId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Get info test by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/test/AB670EFA-9049-46F2-AA3F-8C5044657851
        /// </remarks>
        /// <param name="id">Test id guid</param>
        /// <returns>Returns TestLookupDto</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Couch,Student,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TestLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsTestQuery
            {
                Id = id,
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Create object test 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST (HOST)/api/test
        /// {
        ///   "materialId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///   "title": "string",
        ///   "description": "string"
        /// }
        /// </remarks>
        /// <param name="createTestDto">CreateTestDto object</param>
        /// <returns>Returns id (guid)</returns>
        /// <response code="201">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        /// <response code="403">If the user not have permission</response>
        [HttpPost]
        [Authorize(Roles = "Couch,Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateTestDto createTestDto)
        {
            var command = mapper.Map<CreateTestCommand>(createTestDto);
            command.CurrentUserId = UserId;
            var testId = await Mediator.Send(command);
            return Ok(testId);
        }

        /// <summary>
        /// Update object test 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// PUT (HOST)/api/test
        /// {
        ///   "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///   "title": "string",
        ///   "description": "string"
        /// }
        /// </remarks>
        /// <param name="updateTestDto">UpdateTestDto object</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        /// <response code="403">If the user not have permission</response>
        [HttpPut]
        [Authorize(Roles = "Couch,Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update([FromBody] UpdateTestDto updateTestDto)
        {
            var command = mapper.Map<UpdateTestCommand>(updateTestDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }


        /// <summary>
        /// Delete object test by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE (HOST)/api/test/AB670EFA-9049-46F2-A5BF-8C5044287851
        /// </remarks>
        /// <param name="id">Test id (guid)</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        /// <response code="403">If the user not have permission</response>
        [HttpDelete("{Id}")]
        [Authorize(Roles = "Couch,Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteTestCommand
            {
                Id = id,
                CurrentUserId = UserId
            };

            await Mediator.Send(command);
            return NoContent();
        }
    }
}
