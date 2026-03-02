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
using static Application.Common.Dtos.AnswersUsers.TestResult.CheckingResponsesDto;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AnswersUserController(IMapper mapper) : BaseController
    {
        /// <summary>
        /// Get all answers users in test result by test result id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/answerUser/all/AB670EFA-9049-46F2-A5BF-8C5044287851
        /// </remarks>
        /// <param name="testResultId">Test result id guid</param>
        /// <returns>Returns TestHistoryVm</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("all/{testResultId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TestHistoryVm>> GetTestHistory(Guid testResultId)
        {
            var query = new GetAllAnswersUserQuery()
            {
                CurrentUserId = UserId,
                TestResultId = testResultId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }


        /// <summary>
        /// Get info answer user by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/answerUser/AB670EFA-9049-46F2-AA3F-8C5044657851
        /// </remarks>
        /// <param name="id">Answer user id guid</param>
        /// <returns>Returns AnswersUserLookupDto</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Create object answer user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST (HOST)/api/answerUser
        ///       {
        /// "testResultId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        /// "selectedAnswers": [
        ///   {
        ///     "questionId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///     "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///   }
        /// ],
        /// "completedAt": "2026-02-28T21:08:01.222Z"
        ///
        /// </remarks>
        /// <param name="request">CompleteTestRequestDto object</param>
        /// <returns>Returns id (guid)</returns>
        /// <response code="201">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CompleteTestResponseDto>> Create([FromBody] CompleteTestRequestDto request)
        {
            try
            {
                var command = new CreateAnswersUserCommand
                {
                    CurrentUserId = UserId,
                    TestResultId = request.TestResultId,
                    SelectedAnswers = request.SelectedAnswers,
                    CompletedAt = request.CompletedAt
                };

                var result = await Mediator.Send(command);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Delete object answer user by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE (HOST)/api/answerUser/AB670EFA-9049-46F2-A5BF-8C5044287851
        /// </remarks>
        /// <param name="id">Answer user id (guid)</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
