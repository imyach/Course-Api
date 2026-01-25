using Application.Common.Commands.Courses.CreateCourse;
using Application.Common.Commands.Courses.DeleteCourse;
using Application.Common.Commands.Courses.UpdateCourse;
using Application.Common.Dtos.Courses;
using Application.Common.Queries.Courses.GetCourse;
using Application.Common.Queries.Courses.GetCourseList;
using Application.Common.Queries.Courses.GetCreatedCourse;
using AutoMapper;
using CourseWebApi.Models.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    public class CourseController(IMapper mapper) : BaseController
    {
        [HttpGet("All")]
        public async Task<ActionResult<CourseListVm>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchText = null)
        {
            var query = new GetAllCourseQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchText = searchText
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpGet("Drafted")]
        public async Task<ActionResult<CourseListVm>> GetAllCreated([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllCreatedCoursesQuery
            {
                CurrentUserId = UserId,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Couch,Student")]
        public async Task<ActionResult<CourseLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsCourseQuery
            {
                Id = id,
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Couch")]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateCourseDto createCourseDto)
        {
            var command = mapper.Map<CreateCourseCommand>(createCourseDto);
            command.CurrentUserId = UserId;
            var commandId = await Mediator.Send(command);
            return Ok(commandId);
        }
        [HttpPut]
        [Authorize(Roles = "Admin,Couch")]
        public async Task<IActionResult> Update([FromBody] UpdateCourseDto updateCourseDto )
        {
            var command = mapper.Map<UpdateCourseCommand>(updateCourseDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }
        [HttpDelete("{Id}")]
        [Authorize(Roles = "Admin,Couch")]
        public async Task<IActionResult> Delete(Guid id) 
        {
            var command = new DeleteCourseCommand
            {
                Id = id,
                CurrentUserId = UserId
            };

            await Mediator.Send(command);
            return NoContent();
        }
    }
}
