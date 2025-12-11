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
        public async Task<ActionResult<ReviewListVm>> GetAll()
        {
            var query = new GetAllReviewQuery
            {
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
