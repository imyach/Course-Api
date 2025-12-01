using Application.Common.Commands.Answers.CreateAnswer;
using Application.Common.Commands.Answers.DeleteAnswer;
using Application.Common.Commands.Answers.UpdateAnswer;
using Application.Common.Commands.Courses.CreateCourse;
using Application.Common.Commands.Courses.DeleteCourse;
using Application.Common.Commands.Courses.UpdateCourse;
using Application.Common.Dtos.Answers;
using Application.Common.Dtos.Courses;
using Application.Common.Queries.Answers.GetAnswer;
using Application.Common.Queries.Answers.GetAnswerList;
using Application.Common.Queries.Courses.GetCourse;
using Application.Common.Queries.Courses.GetCourseList;
using AutoMapper;
using CourseWebApi.Models.Answers;
using CourseWebApi.Models.Course;
using Microsoft.AspNetCore.Mvc;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    public class AnswerController(IMapper mapper) : BaseController
    {
        [HttpGet("All")]
        public async Task<ActionResult<AnswerListVm>> GetAll()
        {
            var query = new GetAllAnswerQuery()
            {
                CurrentUserId = UserId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<AnswerLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsAnswerQuery
            {
                Id = id,
                CurrentUserId = UserId
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateAnswerDto createAnswerDto)
        {
            var command = mapper.Map<CreateAnswerCommand>(createAnswerDto);
            command.CurrentUserId = UserId;
            var commandId = await Mediator.Send(command);
            return Ok(commandId);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAnswerDto updateAnswerDto)
        {
            var command = mapper.Map<UpdateAnswerCommand>(updateAnswerDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteAnswerCommand

            {
                Id = id,
                CurrentUserId = UserId
            };

            await Mediator.Send(command);
            return NoContent();
        }
    }
}
