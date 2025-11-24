using Application.Common.Commands.Users.CreateUser;
using Application.Common.Commands.Users.DeteleUser;
using Application.Common.Commands.Users.UpdateUser;
using Application.Common.Dtos.Users;
using Application.Common.Queries.Users.GetUser;
using Application.Common.Queries.Users.GetUsersList;
using AutoMapper;
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
                UserId = UserId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetailsVm>> Get(Guid id)
        {
            var query = new GetDetailsUserQuery()
            {
                Id = id,
                UserId = UserId
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateUserCommand createUserCommand)
        {
            var command = mapper.Map<CreateUserCommand>(createUserCommand);
            command.Id = UserId;
            var userId = await Mediator.Send(command);    
            return Ok(userId);    
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            var command = new DeleteUserCommand()
            {
                Id = id
            };
            await Mediator.Send(command);   
            return NoContent();
        }

        [HttpPut("Update")]
        public async Task<ActionResult<Guid>> Update([FromBody] UpdateUserCommand updateUserCommand)
        {
            var command = mapper.Map<UpdateUserCommand>(updateUserCommand);
            command.Id = UserId;
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
