using Application.Common.Commands.Users.CreateUser;
using Application.Common.Commands.Users.DeteleUser;
using Application.Common.Commands.Users.UpdateUser;
using Application.Common.Dtos.Users;
using Application.Common.Queries.Users.GetUser;
using Application.Common.Queries.Users.GetUsersList;
using AutoMapper;
using CourseWebApi.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace CourseWebApi.Controllers
{
    [Route("api/[controller]")]
    public class UserController(IMapper mapper) : BaseController
    {
        [HttpGet("All")]
        public async Task<ActionResult<UsersListVm>> GetAll()
        {
            var query = new GetAllUsersQuery()
            {
                CurrentUserId = UserId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserLooupDto>> Get(Guid id)
        {
            var query = new GetDetailsUserQuery()
            {
                Id = id,
                CurrentUserId = UserId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateUserDto createUserCommand)

        {
            var command = mapper.Map<CreateUserCommand>(createUserCommand);
            command.CurrentUserId = UserId;
            var userId = await Mediator.Send(command);
            return Ok(userId);
        }


        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteUserCommand()
            {

                Id = id,
                CurrentUserId = UserId
            };
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPut]

        public async Task<IActionResult> Update([FromBody] UpdateUserDto updateUserCommand)
        {
            var command = mapper.Map<UpdateUserCommand>(updateUserCommand);
            command.CurrentUserId = UserId;

            await Mediator.Send(command);
            return NoContent();
        }
    }
}
