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
        [HttpGet("All")]
        [Authorize(Roles = "Couch,Student,Admin")]
        public async Task<ActionResult<TestListVm>> GetAll()
        {
            var query = new GetAllTestQuery()
            {
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Couch,Student,Admin")]
        public async Task<ActionResult<TestLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsTestQuery
            {
                Id = id,
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }
        [HttpPost]
        [Authorize(Roles = "Couch,Admin")]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateTestDto createTestDto)
        {
            var command = mapper.Map<CreateTestCommand>(createTestDto);
            command.CurrentUserId = UserId;
            var testId = await Mediator.Send(command);
            return Ok(testId);
        }
        [HttpPut]
        [Authorize(Roles = "Couch,Admin")]
        public async Task<IActionResult> Update([FromBody] UpdateTestDto updateTestDto)
        {
            var command = mapper.Map<UpdateTestCommand>(updateTestDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }
        [HttpDelete("{Id}")]
        [Authorize(Roles = "Couch,Admin")]
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
