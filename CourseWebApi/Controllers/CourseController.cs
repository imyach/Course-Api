using Application.Common.Commands.Courses.CreateCourse;
using Application.Common.Commands.Courses.DeleteCourse;
using Application.Common.Commands.Courses.UpdateCourse;
using Application.Common.Dtos.Courses;
using Application.Common.Queries.Courses.GetCourse;
using Application.Common.Queries.Courses.GetCourseList;
using AutoMapper;
using CourseWebApi.Models.Course;
using Microsoft.AspNetCore.Mvc;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    public class CourseController(IMapper mapper) : BaseController
    {
        [HttpGet("All")]
        public async Task<ActionResult<CourseListVm>> GetAll()
        {
            var query = new GetAllCourseQuery()
            {
                CurrentUserId = UserId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsCourseQuery
            {
                Id = id,
                CurrentUserId = UserId
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateCourseDto createCourseDto)
        {
            var command = mapper.Map<CreateCourseCommand>(createCourseDto);
            command.CurrentUserId = UserId;
            var userid = await Mediator.Send(command);
            return Ok(userid);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCourseDto updateCourseDto )
        {
            var command = mapper.Map<UpdateCourseCommand>(updateCourseDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }
        [HttpDelete("{Id}")]
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
