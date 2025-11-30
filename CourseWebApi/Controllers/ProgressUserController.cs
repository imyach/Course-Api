using Application.Common.Commands.ProgressUsers.CreateProgressUser;
using Application.Common.Commands.ProgressUsers.DeleteProgressUser;
using Application.Common.Commands.ProgressUsers.UpdateProgressUser;
using Application.Common.Dtos.ProgressUsers;
using Application.Common.Queries.ProgressUsers.GetProgressUser;
using Application.Common.Queries.ProgressUsers.GetProgressUserList;
using AutoMapper;
using CourseWebApi.Models.ProgressUser;
using Microsoft.AspNetCore.Mvc;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    public class ProgressUserController(IMapper mapper) : BaseController
    {
        [HttpGet("All")]
        public async Task<ActionResult<ProgressUserListVm>> GetAll()
        {
            var query = new GetAllProgressUserQuery
            {
                CurrentUserId = UserId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProgressUserLookupDto>> Get(Guid id)
        {
            var query = new GetDetailsProgressUserQuery
            {
                Id = id,
                CurrentUserId = UserId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteProgressUserCommand
            {
                CurrentUserId = UserId,
                Id = id
            };

            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateProgressUserDto createProgressUserDto)
        {
            var command = mapper.Map<CreateProgressUserCommand>(createProgressUserDto);
            command.CurrentUserId = UserId;
            var progressUserID = await Mediator.Send(command);
            return Ok(progressUserID);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateProgressUserDto updateProgressUserDto)
        {
            var command = mapper.Map<UpdateProgressUserCommand>(updateProgressUserDto);
            command.CurrentUserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
