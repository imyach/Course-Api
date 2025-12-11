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
using Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    public class AnswerController(IMapper mapper) : BaseController
    {
        [HttpGet("All")]
        [Authorize(Roles = "Couch,Student,Admin")]
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
        [Authorize(Roles = "Couch,Student,Admin")]
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
        [Authorize(Roles = "Couch,Admin")]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateAnswerDto createAnswerDto)
        {
            var command = mapper.Map<CreateAnswerCommand>(createAnswerDto);
            command.CurrentUserId = UserId;
            var commandId = await Mediator.Send(command);
            return Ok(commandId);
        }
        [HttpPut]
        [Authorize(Roles = "Couch,Admin")]
        public async Task<IActionResult> Update([FromBody] UpdateAnswerDto updateAnswerDto)
        {
            var command = mapper.Map<UpdateAnswerCommand>(updateAnswerDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }
        [HttpDelete("{Id}")]
        [Authorize(Roles = "Couch,Admin")]
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
