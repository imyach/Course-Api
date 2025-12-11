using Application.Common.Commands.Answers.CreateAnswer;
using Application.Common.Commands.AnswersUsers.CreateAnswersUser;
using Application.Common.Commands.AnswersUsers.DeleteAnswersUser;
using Application.Common.Commands.Courses.CreateCourse;
using Application.Common.Commands.Courses.DeleteCourse;
using Application.Common.Commands.Courses.UpdateCourse;
using Application.Common.Dtos.AnswersUsers;
using Application.Common.Dtos.Courses;
using Application.Common.Queries.Answers.GetAnswerList;
using Application.Common.Queries.AnswersUsers.GetAnswersUser;
using Application.Common.Queries.AnswersUsers.GetAnswersUserList;
using Application.Common.Queries.Courses.GetCourse;
using Application.Common.Queries.Courses.GetCourseList;
using AutoMapper;
using CourseWebApi.Models.Answers;
using CourseWebApi.Models.AnswersUsers;
using CourseWebApi.Models.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AnswersUserController(IMapper mapper) : BaseController
    {
        [HttpGet("All")]
        public async Task<ActionResult<AnswersUserListVm>> GetAll()
        {
            var query = new GetAllAnswersUserQuery()
            {
                CurrentUserId = UserId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<AnswersUserLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsAnswersUserQuery
            {
                Id = id,
                CurrentUserId = UserId
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateAnswersUserDto createAnswersUserDto)
        {
            var command = mapper.Map<CreateAnswersUserCommand>(createAnswersUserDto);
            command.CurrentUserId = UserId;
            var commandId = await Mediator.Send(command);
            return Ok(commandId);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteAnswersUserCommand
            {
                Id = id,
                CurrentUserId = UserId
            };

            await Mediator.Send(command);
            return NoContent();
        }
    }
}
