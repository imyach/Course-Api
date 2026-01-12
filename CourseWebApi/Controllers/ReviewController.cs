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
        [HttpGet("All")]
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsReviewQuery
            {
                Id = id,
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpDelete("{id}")]
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

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateReviewsDto createReviewsDto)
        {
            var command = mapper.Map<CreateReviewCommand>(createReviewsDto);
            command.CurrentUserId = UserId;
            var reviewId = await Mediator.Send(command);
            return Ok(reviewId);
        }
    }
}
