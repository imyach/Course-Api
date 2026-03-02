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
        /// <summary>
        /// Get all questions in test by test id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/question/all/AB670EFA-9049-46F2-A5BF-8C5044287851
        /// </remarks>
        /// <param name="testId">Test id guid</param>
        /// <returns>Returns QuestionListVm</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("All/{testId}")]
        [Authorize(Roles = "Couch,Student,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<QuestionListVm>> GetAll(Guid testId)
        {
            var query = new GetAllQuestionQuery
            {
                TestId = testId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Get info question by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/question/AB670EFA-9049-46F2-AA3F-8C5044657851
        /// </remarks>
        /// <param name="id">Question id guid</param>
        /// <returns>Returns QuestionLookupDto</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Couch,Student,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<QuestionLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsQuestionQuery
            {
                Id = id,
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Delete object question by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE (HOST)/api/question/AB670EFA-9049-46F2-A5BF-8C5044287851
        /// </remarks>
        /// <param name="id">Question id (guid)</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        /// <response code="403">If the user not have permission</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Couch,Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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


        /// <summary>
        /// Create object question 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST (HOST)/api/question
        /// {
        ///   "testId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///   "text": "string"
        /// }
        /// </remarks>
        /// <param name="createQuestionDto">CreateQuestionDto object</param>
        /// <returns>Returns id (guid)</returns>
        /// <response code="201">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        /// <response code="403">If the user not have permission</response>
        [HttpPost]
        [Authorize(Roles = "Couch,Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateQuestionDto createQuestionDto)
        {
            var command = mapper.Map<CreateQuestionCommand>(createQuestionDto);
            command.CurrentUserId = UserId;
            var questionId = await Mediator.Send(command);
            return Ok(questionId);
        }

        /// <summary>
        /// Update object question 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// PUT (HOST)/api/question
        /// {
        ///   "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///   "text": "string"
        /// }
        /// </remarks>
        /// <param name="updateQuestionDto">UpdateQuestionDto object</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        /// <response code="403">If the user not have permission</response>
        [HttpPut]
        [Authorize(Roles = "Couch,Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update([FromBody] UpdateQuestionDto updateQuestionDto)
        {
            var command = mapper.Map<UpdateQuestionCommand>(updateQuestionDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
