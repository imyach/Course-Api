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
        /// <summary>
        /// Get all answers in question by question id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/answer/all/AB670EFA-9049-46F2-A5BF-8C5044287851
        /// </remarks>
        /// <param name="questionId">Question id guid</param>
        /// <returns>Returns AnswerListVm</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("All/{questionId}")]
        [Authorize(Roles = "Couch,Student,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AnswerListVm>> GetAll(Guid questionId)
        {
            var query = new GetAllAnswerQuery
            {
                QuestionId = questionId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Get info answer by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/answer/AB670EFA-9049-46F2-AA3F-8C5044657851
        /// </remarks>
        /// <param name="id">Answer id guid</param>
        /// <returns>Returns AnswerLookupDto</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Couch,Student,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AnswerLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsAnswerQuery
            {
                Id = id,
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Create object answer 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST (HOST)/api/answer
        /// {
        ///     "text": "string",
        ///     "questionId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///     "isCorrect": true
        /// }
        /// </remarks>
        /// <param name="createAnswerDto">CreateAnswerDto object</param>
        /// <returns>Returns id (guid)</returns>
        /// <response code="201">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        /// <response code="403">If the user not have permission</response>
        [HttpPost]
        [Authorize(Roles = "Couch,Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateAnswerDto createAnswerDto)
        {
            var command = mapper.Map<CreateAnswerCommand>(createAnswerDto);
            command.CurrentUserId = UserId;
            var commandId = await Mediator.Send(command);
            return Ok(commandId);
        }

        /// <summary>
        /// Update object answer 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// PUT (HOST)/api/answer
        /// {
        ///     "id": "5da85f64-5347-4562-b3fc-2645df66afc5",
        ///     "text": "string",
        ///     "isCorrect": false
        /// }
        /// </remarks>
        /// <param name="updateAnswerDto">UpdateAnswerDto object</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        /// <response code="403">If the user not have permission</response>
        [HttpPut]
        [Authorize(Roles = "Couch,Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update([FromBody] UpdateAnswerDto updateAnswerDto)
        {
            var command = mapper.Map<UpdateAnswerCommand>(updateAnswerDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Delete object answer by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE (HOST)/api/answer/AB670EFA-9049-46F2-A5BF-8C5044287851
        /// </remarks>
        /// <param name="id">Answer id (guid)</param>
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
