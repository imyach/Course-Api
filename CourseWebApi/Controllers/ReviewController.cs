using Application.Common.Commands.Modules.CreateModule;
using Application.Common.Commands.Modules.DeleteModule;
using Application.Common.Commands.Modules.UpdateModule;
using Application.Common.Commands.Rewies.CreateReview;
using Application.Common.Commands.Rewies.DeleteReview;
using Application.Common.Dtos.Modules;
using Application.Common.Dtos.Reviews;
using Application.Common.Queries.Modules.GetModule;
using Application.Common.Queries.Modules.GetModuleList;
using Application.Common.Queries.Reviews.GetReview;
using Application.Common.Queries.Reviews.GetReviewList;
using AutoMapper;
using CourseWebApi.Models.Module;
using CourseWebApi.Models.Reviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Couch,Student")]
    public class ReviewController(IMapper mapper) : BaseController
    {
        /// <summary>
        /// Get reviews elements
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/review/all
        /// </remarks>
        /// <param name="pageNumber">Page Number </param>
        /// <param name="pageSize">The number of items returned by the server</param>
        /// <param name="sortAscending">Sort ascending items</param>
        /// <param name="sortBy">Sort param items</param>
        /// <returns>Returns ReviewListVm</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ReviewListVm>> GetAll([FromQuery] Guid idCourse, 
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10,
            [FromQuery] bool sortAscending = false,
            [FromQuery] string sortBy = null)
        {
            var query = new GetAllReviewQuery
            {
                IdCourse = idCourse,
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortAscending = sortAscending,
                SortBy = sortBy
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Get info review by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET (HOST)/api/review/AB670EFA-9049-46F2-AA3F-8C5044657851
        /// </remarks>
        /// <param name="id">Review id guid</param>
        /// <returns>Returns ReviewLookupDto</returns>
        /// <response code="200">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ReviewLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsReviewQuery
            {
                Id = id,
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Delete object review by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE (HOST)/api/review/AB670EFA-9049-46F2-A5BF-8C5044287851
        /// </remarks>
        /// <param name="id">Review id (guid)</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteReviewCommand
            {
                Id = id,
                CurrentUserId = UserId
            };
            await Mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Create object review 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST (HOST)/api/review
        /// {
        ///   "courseId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///   "rait": 0,
        ///   "text": "string"
        /// }
        /// </remarks>
        /// <param name="createReviewsDto">CreateReviewsDto object</param>
        /// <returns>Returns id (guid)</returns>
        /// <response code="201">Siccess</response>
        /// <response code="401">If the user is unautorized</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateReviewsDto createReviewsDto)
        {
            var command = mapper.Map<CreateReviewCommand>(createReviewsDto);
            command.CurrentUserId = UserId;
            var reviewId = await Mediator.Send(command);
            return Ok(reviewId);
        }
    }
}
