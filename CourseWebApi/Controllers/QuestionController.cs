using Application.Common.Commands.ProgressUsers.CreateProgressUser;
using Application.Common.Commands.ProgressUsers.DeleteProgressUser;
using Application.Common.Commands.ProgressUsers.UpdateProgressUser;
using Application.Common.Commands.Questions.CreateQuestion;
using Application.Common.Commands.Questions.DeleteQuestion;
using Application.Common.Commands.Questions.UpdateQuestion;
using Application.Common.Dtos.ProgressUsers;
using Application.Common.Dtos.Questions;
using Application.Common.Queries.ProgressUsers.GetProgressUser;
using Application.Common.Queries.ProgressUsers.GetProgressUserList;
using Application.Common.Queries.Questions.GetQuestionList;
using Application.Common.Queries.Questions.GetQuestions;
using AutoMapper;
using CourseWebApi.Models.ProgressUser;
using CourseWebApi.Models.Questions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    public class QuestionController(IMapper mapper) : BaseController
    {
        [HttpGet("All/{testId}")]
        [Authorize(Roles = "Couch,Student,Admin")]
        public async Task<ActionResult<QuestionListVm>> GetAll(Guid testId)
        {
            var query = new GetAllQuestionQuery
            {
                TestId = testId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Couch,Student,Admin")]
        public async Task<ActionResult<QuestionLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsQuestionQuery
            {
                Id = id,
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Couch,Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteQuestionCommand
            {
                CurrentUserId = UserId,
                Id = id
            };

            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Couch,Admin")]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateQuestionDto createQuestionDto)
        {
            var command = mapper.Map<CreateQuestionCommand>(createQuestionDto);
            command.CurrentUserId = UserId;
            var questionId = await Mediator.Send(command);
            return Ok(questionId);
        }

        [HttpPut]
        [Authorize(Roles = "Couch,Admin")]
        public async Task<IActionResult> Update([FromBody] UpdateQuestionDto updateQuestionDto)
        {
            var command = mapper.Map<UpdateQuestionCommand>(updateQuestionDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
